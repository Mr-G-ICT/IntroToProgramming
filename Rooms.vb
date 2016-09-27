Imports System.IO
Module Rooms
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'This module focuses around the room algorithms and things you can do in the room.
    'Author: B Gristwood
    'Date 11/2/14
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Function GetRoom(ByVal RoomNumber, ByVal CurrentRoom)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name : Getroom
        'Description: This finds the rooms from the text file and puts them into the structure so they can be used in the rest of the program
        'Inputs: the room that you are travelling to, the structure currentroom that holds the room data
        'Outputs: Returns all the attributes in currentroom.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim RoomArray(8) As String
        Dim Counter As Integer = 1
        Dim FilePath As New FileStream(CharacterGeneration.RoomsFile, FileMode.Open, FileAccess.ReadWrite)
        Dim FileReader As New StreamReader(FilePath)

        FileReader.BaseStream.Seek(0, SeekOrigin.Begin)
        While FileReader.Peek() > -1

            If FileReader.ReadLine() = RoomNumber Then
                'EOR is an end of room marker in the file, used as some rooms have more in than others.
                While Counter <> 7 And RoomArray(Counter - 1) <> "EOR"
                    RoomArray(Counter) = FileReader.ReadLine()
                    Counter = Counter + 1
                End While
                Exit While
            End If
        End While

        CurrentRoom.RoomStructure = RoomArray(1)
        CurrentRoom.RoomOptions = RoomArray(2)
        CurrentRoom.RoomDirect = RoomArray(3)
        CurrentRoom.Monsters = RoomArray(4)

        FileReader.Close()
        Return (CurrentRoom)
    End Function

    Sub DisplayRoom(ByVal CurrentRoom, ByVal Maincharacter)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name : DisplayRoom
        'Description: A recursive function that displays the room the player is in, accepts commands and quits on death.
        'Inputs: The CurrentRoom they are in.
        'Outputs: None
        'Extends : DisplayRoom, GetRoom
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Counter As Integer = 0
        Dim TotalArguments As Integer
        Dim match As Boolean = False
        Dim RoomAction As String = ""
        Dim roomcommands() As String
        Dim redirect() As String
        Dim Monster As character
        Dim Improvement As Integer

        roomcommands = Split(CurrentRoom.RoomOptions, ",", , CompareMethod.Text)
        redirect = Split(CurrentRoom.roomdirect, ",", , CompareMethod.Text)
        Console.WriteLine(CurrentRoom.RoomStructure)

        If redirect(0) = "DEAD" Then
            Console.Write("you are dead")
        Else
            'Formats and displays the possible room commands
            While Counter <> roomcommands.Length
                roomcommands(Counter) = Replace(roomcommands(Counter), ",", "")
                roomcommands(Counter) = RTrim(roomcommands(Counter))
                roomcommands(Counter) = LTrim(roomcommands(Counter))
                Console.WriteLine(roomcommands(Counter))
                Counter = Counter + 1
            End While


            'Fights will probably need to be built in somewhere here.
            If CurrentRoom.Monsters <> "NULL" Then
                'pick a random monster from the monster file in order to start a fight
                Monster = GenerateMonster(Monster, CurrentRoom.Monsters)

                'Call the fight algorithm, if you win, you add the modifier to your original strength, making you stronger
                Improvement = Fights.FightClub(Maincharacter.Name, Maincharacter.Strength, Monster.Strength, Maincharacter.Skill, Monster.Skill, Monster.Name)

                If Improvement <> -1 Then
                    Maincharacter.Strength = Maincharacter.Strength + Improvement
                    'this bit will then redirect you to the option you have chosen
                    CurrentRoom = GetRoom(redirect(0), CurrentRoom)
                    DisplayRoom(CurrentRoom, Maincharacter)
                    match = True
                Else
                    Console.WriteLine("GAME OVER!!")
                    roomcommands(0) = "DEAD"
                End If

            End If

            'Loop to make sure command is entered correctly.
            While match <> True And roomcommands(0) <> "DEAD"
                Console.WriteLine("what would you like to do?")
                RoomAction = Console.ReadLine()

                'check that the argument is in the command list, then redirect accordingly
                TotalArguments = Counter
                Counter = 0
                While Counter <> TotalArguments

                    'IF Statement that ends recursion, Go to new room or End game argument
                    If roomcommands(Counter) = "DEAD" Then
                        Console.WriteLine("end of game")
                        Exit While

                    ElseIf roomcommands(Counter) = RoomAction Then
                        'display the action, possibly only used for testing, but could be kept to check for spelling errors.
                        Console.WriteLine("you have chosen to " & RoomAction)


                        'this bit will then redirect you to the option you have chosen
                        CurrentRoom = GetRoom(redirect(Counter), CurrentRoom)
                        DisplayRoom(CurrentRoom, Maincharacter)
                        match = True
                    End If
                    Counter = Counter + 1
                End While

                'if command is unknown
                If match = False And Counter = TotalArguments Then
                    Console.WriteLine("sorry i don't know what to do?")
                End If
            End While
        End If
    End Sub

End Module
