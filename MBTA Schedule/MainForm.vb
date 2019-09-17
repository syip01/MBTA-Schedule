'   Simon Yip
'   Spring 2018 Post-Code-A-Thon Advanced Challenge
'   Started 3/26/18
'   Submitted 4/12/18
'   Version v3.1
'   v1 was basic - commuter rail only
'   v2 improved GUI options And parsing
'   v3 expanded parsing plus aliases
'   v3.1 fixing odd bugs due to probably MBTA formatting changes

Option Explicit On
Option Strict On
Option Infer Off

Imports System.IO
Imports System.Net

Public Class frmMain
    Const BaseWebLink As String = "https://api-v3.mbta.com"         'used for parsing
    Private CreatedWebLink As String = String.Empty                 'stored constructed link for processing
    Private JsonFilename As String = "mbta.json"                    'filename for Json file
    Private LocAliasFilename As String = "mbta_alias.txt"           'filename for alias hash file
    Private SearchType As String = "Prediction"                     'type of search
    Private SortType As String = "Departure Time"                   'the sort used on data
    'Private SortType As String = "Arrival Time"                    'old sort due to "North Station" behavior
    Private SortAscend As Boolean = True                            'sort direction, defaults to ascend
    Private Direction As Integer = 0                                '0 = outbound, 1 = inbound
    Private TrainStop As String = "Quincy Center"                   'train stop used in the query
    Private EnableAPIKey As Boolean = True                          'default on
    Private API_Key As String = "36a29d2bb8474699a79e9f859b85daa4"  'my personal API key
    Private TimerTest As Boolean = False                            'used for personal testing
    Private TimerTestInterval As Integer = 1000                     'timer interval for queries in testing is 1 sec, normally 6 sec
    Private LocArray As New ArrayList                               'stores location data
    Private AliasArray As New ArrayList                             'stores location aliases

    Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
        GetJsonData()
        ProcessFile()                                               'process file

    End Sub

    Private Sub GetJsonData()
        'Login info found in provided link - and other MSDN resources provides the same stuff
        'https://forums.asp.net/t/2059912.aspx?VB+Net+Get+API+JSON
        Dim workspace As String                                     'stores the data/response received from link

        Dim MyCredentials As System.Net.NetworkCredential           ' define default app credentials

        'AFAIK, we don't need any real credentials
        MyCredentials = New System.Net.NetworkCredential(userName:="...", password:="...")

        'default response w/o API key, based on documentation
        'Dim request As WebRequest = WebRequest.Create("https://api-v3.mbta.com/predictions?sort=arrival_time&filter%5Bdirection_id%5D=0&filter%5Bstop%5D=Quincy%20Center")
        'using custom response for now, will make it more flexible later
        Dim request As WebRequest = WebRequest.Create(CreatedWebLink)

        request.Method = "GET"                                      'GET response from website
        request.Credentials = MyCredentials                         ' set default credentials
        Dim response As WebResponse = request.GetResponse()         ' get response from intended website

        Dim JsonStream As Stream = response.GetResponseStream()     '' define the stream(must be stream)
        Dim reader As New StreamReader(JsonStream)                  '' get the data stream set
        workspace = reader.ReadToEnd                                ' get all the data chunks
        JsonStream.Dispose()                                        '' get rid of the stream, we no longer need it
        reader.Close()                                              ' close stuff that's not needed
        response.Close()                                            'end session

        Dim JsonFile As System.IO.StreamWriter                      'have a streamwriter to get the data
        JsonFile = My.Computer.FileSystem.OpenTextFileWriter(JsonFilename, False)   'create file, always overwrite
        JsonFile.WriteLine(workspace)                               'write each line returned from the data stream
        JsonFile.Close()                                            'close file

    End Sub

    'Start parsing/chunking the file that was created
    Private Sub ProcessFile()
        'store entire file into a string
        Dim readJson As String
        'read entire file into a string
        readJson = My.Computer.FileSystem.ReadAllText(JsonFilename)
        'clear all content from listBox
        lstSched.Items.Clear()
        'attempt to parse data in chunks
        While (readJson.Contains("""id"""))
            ProcessChunk(readJson)
        End While

    End Sub

    'Processes chunks of the file, "4 ids @ a time"
    Private Sub ProcessChunk(ByRef data As String)
        Dim routeText As String = String.Empty
        Dim dirText As String = String.Empty
        Dim timeText As String = String.Empty
        'store data to lookup
        Dim strLookup As String
        Dim comRail As Boolean
        Dim dirInfo As String = String.Empty                        'stores direction info

        'get arrival time
        ' apparently arrival_time is "unreliable' @ North Station
        'strLookup = """arrival_time"""
        strLookup = """departure_time"""
        ChunkString(data, strLookup)

        'get info from arrival time when departure time is null
        If (data.Substring(0, 5) = ":null") Then
            strLookup = """arrival time"""
            ChunkString(data, strLookup)
        End If

        'move to "T"
        strLookup = "T"
        ChunkString(data, strLookup)
        timeText += "Time: " + TimeOfDay(data.Substring(0, 5)) + " " + TimeZone(data.Substring(8, 6)) + " "
        routeText = timeText + routeText

        'get direction
        strLookup = """direction_id"":"
        ChunkString(data, strLookup)

        'process direction
        dirText += "Dir: "
        dirInfo = data.Substring(0, 1)
        If (dirInfo = "0") Then
            dirText += "OUT "
        ElseIf (dirInfo = "1") Then
            dirText += "IN     "
        End If
        routeText = dirText + routeText

        'look for "id" (#1)
        strLookup = """id"""
        ChunkString(data, strLookup)

        'head to next "id" (id #2)
        strLookup = """id"""
        ChunkString(data, strLookup)

        'determine whether connection is commuter rail
        strLookup = ":"""
        ChunkString(data, strLookup)

        comRail = data.Substring(0, 2) = "CR"
        If (comRail) Then
            ProcessCRSched(data, routeText)
        Else
            ProcessRailBusSched(data, routeText, dirInfo)
        End If

        'add to list
        lstSched.Items.Add(routeText)

    End Sub

    ' Remove all unwanted parts of the string up to the point of where we want to start
    Private Sub ChunkString(ByRef data As String, ByVal strLookup As String)
        Dim index As Integer
        'get index of string to lookup
        index = data.IndexOf(strLookup)
        'remove everything up to and including the first instance of the string
        'data = data.Substring(index + strLookup.Length, data.Length - (index + strLookup.Length))
        data = data.Remove(0, index + strLookup.Length)

    End Sub

    ' Converts 24 hour to 12 hour
    Function TimeOfDay(ByVal time As String) As String
        Dim newTime As String = String.Empty
        Dim hour As Integer

        Integer.TryParse(time.Substring(0, 2), hour)
        If (hour >= 22) Then
            newTime += (hour Mod 12).ToString()
        ElseIf (hour > 12) Then
            newTime += (hour Mod 12).ToString().PadLeft(3, CChar(" "))
        ElseIf (hour = 0) Then
            newTime += "12"
        ElseIf (hour >= 10) Then
            newTime += hour.ToString()
        Else
            newTime += hour.ToString().PadLeft(3, CChar(" "))
        End If

        newTime += time.Substring(2, 3)

        If (hour > 11) Then
            newTime += "PM"
        Else
            newTime += "AM"
        End If

        Return (newTime)

    End Function

    'Return Time Zone - only will be EST/EDT due to locality
    Function TimeZone(ByVal time As String) As String
        If (time.Contains("-04:00")) Then
            Return ("EDT")
        ElseIf (time.Contains("-05:00")) Then
            Return ("EST")
        Else
            Return ("")
        End If

    End Function

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'set default Listbox selection
        cboDest.SelectedIndex = 0

        'load location aliases
        CreateLocationTable()

        If (TimerTest) Then
            tmrUpdate.Interval = TimerTestInterval
        End If
        'Generate link to download data
        ParseLinkRequest()
        'Get JSON data
        GetJsonData()
        'Process JSON data
        ProcessFile()

    End Sub

    'sets CreatedWebLink to appropriate data, "incomplete"/limited
    Private Sub ParseLinkRequest()
        'start with base link
        CreatedWebLink = BaseWebLink

        'deal with search type
        If SearchType = "Prediction" Then
            CreatedWebLink += "/predictions?"
        End If

        'deal with sort 
        Dim sortTypeParse As String = SortType.Replace(" ", "_").ToLower()

        If SortType = "Departure Time" Then
            If (Not SortAscend) Then
                sortTypeParse = "-" + sortTypeParse
            End If
            CreatedWebLink += "sort=" + sortTypeParse
        ElseIf SortType = "Arrival Time" Then
            If (Not SortAscend) Then
                sortTypeParse = "-" + sortTypeParse
            End If
            CreatedWebLink += "sort=" + sortTypeParse
        End If

        'deal with direction, 0 = outbound, 1 = inbound
        If (Direction = 0 OrElse Direction = 1) Then
            CreatedWebLink += "&filter%5Bdirection_id%5D=" + Direction.ToString()
        End If

        'deal with train stop
        If (TrainStop <> String.Empty) Then
            If (LocArray.Contains(TrainStop.ToUpper)) Then
                CreatedWebLink += "&filter%5Bstop%5D=" + AliasArray(LocArray.IndexOf(TrainStop.ToUpper)).ToString
            Else
                CreatedWebLink += "&filter%5Bstop%5D=" + TrainStop.Replace(" ", "%20")
            End If
        End If

        'deal with inserting API key
        If (EnableAPIKey) Then
            CreatedWebLink += "&api_key=" + API_Key
        End If

    End Sub

    Private Sub radDirBoth_CheckedChanged(sender As Object, e As EventArgs) Handles radDirBoth.CheckedChanged
        Direction = -1
        'update link data
        linkUpdate()

    End Sub

    Private Sub radDirOut_CheckedChanged(sender As Object, e As EventArgs) Handles radDirOut.CheckedChanged
        Direction = 0
        'update link data
        linkUpdate()
    End Sub

    Private Sub radDirIn_CheckedChanged(sender As Object, e As EventArgs) Handles radDirIn.CheckedChanged
        Direction = 1
        'update link data
        linkUpdate()
    End Sub

    Private Sub radListAscend_CheckedChanged(sender As Object, e As EventArgs) Handles radSortAscend.CheckedChanged
        SortAscend = True
        'update link data
        linkUpdate()
    End Sub

    Private Sub radSortDescend_CheckedChanged(sender As Object, e As EventArgs) Handles radSortDescend.CheckedChanged
        SortAscend = False
        'update link data
        linkUpdate()
    End Sub

    Private Sub cboDest_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles cboDest.KeyDown
        If e.KeyCode = Keys.Enter Then
            TrainStop = cboDest.Text
            'update link data
            linkUpdate()
        End If
    End Sub

    Private Sub cboDest_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDest.SelectedIndexChanged
        TrainStop = cboDest.Text
        'update link data
        linkUpdate()
    End Sub

    'Attempt to avoid race condition
    Private Sub linkUpdate()
        'stop timer
        tmrUpdate.Stop()
        'update link data
        ParseLinkRequest()
        'Get JSON data
        GetJsonData()
        'Process JSON data
        ProcessFile()
        'start timer
        tmrUpdate.Start()

    End Sub

    Private Sub ProcessCRSched(ByRef data As String, ByRef trainText As String)
        'store data to lookup
        Dim strLookup As String
        'store index of found data
        Dim index As Integer

        Dim dayInfo As String = String.Empty
        Dim seasonInfo As String = String.Empty
        Dim yearInfo As String = String.Empty
        Dim lineInfo As String = String.Empty

        'add Commuter Rail tag to data
        trainText += "Commuter Rail: "
        'look for CR, remove CR from data
        strLookup = "CR-"
        ChunkString(data, strLookup)

        'get everything until the next double quote
        strLookup = """"
        index = data.IndexOf(strLookup)
        lineInfo = data.Substring(0, index)

        ChunkString(data, strLookup)

        'skip to next "id" (id #3)
        strLookup = """id"""
        ChunkString(data, strLookup)

        'skip to last "id" (id #4)
        strLookup = """id"""
        ChunkString(data, strLookup)

        'store day type schedule string, unused for now
        strLookup = "-"
        index = data.IndexOf(strLookup)
        ChunkString(data, strLookup)

        index = data.IndexOf(strLookup)
        dayInfo = data.Substring(0, index)
        ChunkString(data, strLookup)

        'get season in the string, unused for now
        'strLookup = "-"    implied
        index = data.IndexOf(strLookup)
        seasonInfo = data.Substring(0, index)
        ChunkString(data, strLookup)

        'get year info, unused for now
        'get rid of -YY- where YY is the year
        'strLookup = "-"    implied
        index = data.IndexOf(strLookup)
        yearInfo = data.Substring(0, index)
        ChunkString(data, strLookup)

        'get train route #, look for the double quote
        strLookup = """"
        index = data.IndexOf(strLookup)
        trainText += "Train #" + data.Substring(0, index)
        ChunkString(data, strLookup)

        'parse line info here
        trainText += " Line: " + lineInfo

    End Sub

    Private Sub ProcessRailBusSched(ByRef data As String, ByRef routeText As String, ByVal dirInfo As String)
        'store data to lookup
        Dim strLookup As String
        'store index of found data
        Dim index As Integer
        Dim routeInfo As String = String.Empty                      'stores route info
        Dim stopInfo As String = String.Empty                       'stores stop info

        'get route info
        strLookup = """"
        index = data.IndexOf(strLookup)
        routeInfo = data.Substring(0, index)

        Select Case routeInfo
            Case "Red",
            "Mattapan",
            "Green-B",
            "Green-C",
            "Green-D",
            "Green-E",
            "Orange",
            "Blue"
                routeText += routeInfo + " Line "
            Case "741"
                routeText += "Silver Line: SL1"
            Case "742"
                routeText += "Silver Line: SL2"
            Case "743"
                routeText += "Silver Line: SL3"
            Case "751"
                routeText += "Silver Line: SL4"
            Case "749"
                routeText += "Silver Line: SL5"
            Case "746"
                routeText += "Silver Line: Waterfront Shuttle"
            Case "701"
                routeText += "Bus Route: CT1"
            Case "747"
                routeText += "Bus Route: CT2"
            Case "708"
                routeText += "Bus Route: CT3"
            Case Else
                routeText += "Bus Route: " + routeInfo
        End Select

        ChunkString(data, strLookup)

        'move to id #3
        strLookup = """id"""
        ChunkString(data, strLookup)

        'get stop info
        strLookup = """"      'remove everything up to and including the double quotes
        ChunkString(data, strLookup)

        index = data.IndexOf(strLookup)
        stopInfo = data.Substring(0, index)
        ChunkString(data, strLookup)    'clean up after parsing it

        'skip to next/last "id" of the set (id #4)
        strLookup = """id"""    'determine bus or train from this info
        ChunkString(data, strLookup)

        GetTrainDest(routeText, routeInfo, stopInfo, dirInfo)

    End Sub

    'Get destination of non-commuter rail train
    Private Sub GetTrainDest(ByRef routeText As String, ByVal routeInfo As String,
                             ByVal stopInfo As String, ByVal dirInfo As String)
        Dim destText As String = String.Empty

        Select Case routeInfo
            Case "Red"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    AshmontOrBraintree(destText, stopInfo)
                Else
                    destText += "Alewife"
                End If
            Case "Mattapan"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Mattapan"
                End If
            Case "Green-B"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Boston College"
                Else
                    destText += "Park Street"
                End If
            Case "Green-C"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Cleveland Circle"
                Else
                    destText += "North Station"
                End If
            Case "Green-D"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Riverside"
                Else
                    destText += "Government Center"
                End If
            Case "Green-E"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Heath Street"
                Else
                    destText += "Lechmere"
                End If
            Case "Orange"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Forest Hills"
                Else
                    destText += "Oak Grove"
                End If
            Case "Blue"
                destText += "Dest: "
                If (dirInfo = "0") Then
                    destText += "Bowdoin"
                Else
                    destText += "Wonderland"
                End If
        End Select

        routeText += destText

    End Sub

    'adds to destination text to whether train destination is Ashmont or Braintree
    Private Sub AshmontOrBraintree(ByRef destText As String, ByVal stopInfo As String)
        'route info is specific to JFK/UMass
        If (stopInfo = "70085") Then
            destText += "Ashmont"
            Return
        ElseIf (stopInfo = "70095") Then
            destText += "Braintree"
            Return
        End If

        Select Case TrainStop
            Case "Ashmont", "place-asmnl",
                 "Shawmut", "place-smmnl",
                 "Fields Corner", "place-fldcr",
                 "Savin Hill", "place-shmnl"
                destText += "Ashmont"
            Case "Braintree", "place-brntn",
                 "Quincy Adams", "place-qamnl",
                 "Quincy Center", "place-qnctr",
                 "North Quincy", "place-nqncy"
                destText += "Braintree"
            Case Else
                'since we're not able to determine route due to data provided
                'assume train always heads to JFK/UMass
                destText += "JFK/UMass"
        End Select
    End Sub

    Private Sub CreateLocationTable()
        'store entire file into a string
        Dim readAlias As String
        'read entire file into a string
        readAlias = My.Computer.FileSystem.ReadAllText(LocAliasFilename)
        'store stuff for lookup
        Dim strLookup As String
        Dim index As Integer

        Dim key As String
        Dim location As String

        While (readAlias <> String.Empty)
            If (readAlias.Substring(0, 1) = "#") Then
                strLookup = Environment.NewLine
                ChunkString(readAlias, strLookup)
            Else
                'get key
                strLookup = "|"
                index = readAlias.IndexOf(strLookup)
                key = readAlias.Substring(0, index).ToUpper
                ChunkString(readAlias, strLookup)
                LocArray.Add(key)
                'get location/value
                strLookup = Environment.NewLine
                index = readAlias.IndexOf(strLookup)
                location = readAlias.Substring(0, index)
                ChunkString(readAlias, strLookup)
                AliasArray.Add(location)
            End If
        End While

    End Sub

    Private Sub radDepart_CheckedChanged(sender As Object, e As EventArgs) Handles radDepart.CheckedChanged
        SortType = "Departure Time"
        'update link data
        linkUpdate()
    End Sub

    Private Sub radArrive_CheckedChanged(sender As Object, e As EventArgs) Handles radArrive.CheckedChanged
        SortType = "Arrival Time"
        'update link data
        linkUpdate()
    End Sub
End Class
