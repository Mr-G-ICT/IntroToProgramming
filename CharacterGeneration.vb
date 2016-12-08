Imports System.IO

Module CharacterGeneration
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Name: Determining Character's Attributes
    'description: Program that calculates the character's attributes
    'Author: B Gristwood
    'Date: 30/01/14
    '
    'KNOWN ISSUES
    'currently the only fight available is the room guard
    'the rooms are half built and you can move around (a bit)
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 

    'justifying use of files:
    'I could have used a database in this instance, i have chosen files for simplicity at school as restrictions on the networks can
    'cause VB to behave wierd, In the files i've given elements that require a primary key which allows me to transfer to a database
    'at a later date.
    Public Const RoomsFile As String = "Roomfile.txt"
    Const CharFilename As String = "Characters.txt"
    'The monster file contains lists of random monsters, each with their own strength and skill
    Const MonsterFile As String = "MonsterFile.txt"

    'this is a standard structure that stores charicter attributes, made as an array so can
    'shrink or grow as the game develops
    Public Structure character
        Dim Name As String
        Dim Strength As Integer
        Dim Skill As Integer
    End Structure

    ' This structure is for the rooms. I've done this so that i can add more details to it later, such as enemies in rooms
    Public Structure rooms
        Dim RoomName As String
        Dim RoomStructure As String
        Dim RoomOptions As String
        Dim RoomDirect As String
        Dim Monsters As String
    End Structure

    'Constants to show the maximum number of monsters and maximum size of array i will need. used with the character structure
    'could possibly redevelop with a bit of work.
    Const ArraySize As Integer = 2
    Const MaxMonsters As Integer = 5

    Sub Main()
        Dim CurrentRoom As Rooms
        Dim monster As character
        Dim MainCharacter As character
        Dim response As Integer

        Dim FightAgain As String = "yes"

        Dim RoomNo As String = "R1"


        'big conditional while, the aim here is to have one charicter for the game that you 
        'load or create. If the array is blank you haven't loaded or created a charicter
        While MainCharacter.Name = ""
            Console.WriteLine("welcome to the ultimate adventure game")
            Console.WriteLine("what would you like to do?")
            Console.WriteLine("1) Generate a new character")
            Console.WriteLine("2) Use Existing Character")
            response = Console.ReadLine

            'case statement to determine what to do with the response, or display error if not valid
            Select Case response
                Case 1
                    MainCharacter = GenerateCharacter(MainCharacter)
                Case 2
                    MainCharacter = LoadCharacter(MainCharacter)
                Case Else
                    Console.WriteLine("please enter a valid choice")
            End Select

        End While

        Console.WriteLine("Welcome " & MainCharacter.Name & " to the game. your current strength is " & MainCharacter.Strength & " and your current skill is " & MainCharacter.Skill)

        CurrentRoom = GetRoom(RoomNo, CurrentRoom)

        DisplayRoom(CurrentRoom, MainCharacter)

        Console.ReadLine()

    End Sub

    Function GenerateMonster(ByVal Monster, ByVal MonsterCode)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name: GenerateMonster
        'Description: Generate a random monster from a text file for the character to be attacked by
        ' I have used a text file rather than a database for the simple reason it is not easy to do
        'in my school
        'Inputs: 
        'Outputs: Monster Structure
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Randomize()

        'initialise variables
        Dim RandomNum As New Random
        ' Dim monstercode As Integer = RandomNum.Next(1, MaxMonsters)

        'Pick a random Number between 1 and the max number of monsters in the file
        Dim CharacterArray(ArraySize) As String
        Dim Counter As Integer = 0
        Dim FilePath As New FileStream(MonsterFile, FileMode.Open, FileAccess.ReadWrite)
        Dim FileReader As New StreamReader(FilePath)

        FileReader.BaseStream.Seek(0, SeekOrigin.Begin)
        While FileReader.Peek() > -1

            If FileReader.ReadLine() = MonsterCode Then
                While Counter <> ArraySize
                    CharacterArray(Counter) = FileReader.ReadLine()
                    Counter = Counter + 1
                End While
                Exit While
            End If

        End While

        Monster.Name = CharacterArray(0)
        Monster.Strength = CharacterArray(1)
        Monster.Skill = CharacterArray(2)

        Return (Monster)
    End Function

    Function GenerateCharacter(ByVal MainCharacter)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name: GenerateCharacter
        'Description: generate the charicter's name and attributes before saving
        'Inputs: None
        'Outpus: Name, Strength and Skill as a single variable as functions can only return one value
        'Extends : CalculateAttribute, Save Charicter
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Console.WriteLine("please enter the character name")
        MainCharacter.Name = Console.ReadLine()

        'Use the calculate attribute function to generate an initial strength and skil value
        MainCharacter.Strength = CalculateAttribute()

        Console.WriteLine("strength is" & MainCharacter.Strength)

        MainCharacter.Skill = CalculateAttribute()

        Console.WriteLine("skill is" & MainCharacter.Skill)


        'Call Save Character to save the value to a file
        SaveCharacter(MainCharacter)

        Return (MainCharacter)

    End Function

    Function CalculateAttribute()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Name : Calculate Attribute
        ' Description : Calculate the attribtes for a charicter
        ' Inputs: None
        ' Outputs A Variable containing the attribute
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Randomize()

        'initialise variables
        Dim RandomNum As New Random

        Dim attribute As Integer = 10

        Dim foursided As Integer
        Dim twelvesided As Integer



        'Generate a random number for the dice
        foursided = RandomNum.Next(1, 4)
        twelvesided = RandomNum.Next(1, 12)

        Return attribute + (twelvesided / foursided)

    End Function


    Sub SaveCharacter(ByVal MainCharacter)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name : SaveCharacter
        'Description: Saves the characters details to a file. as a standard game only has one player
        'overwrite their details each time they save
        'Inputs: the name, strength and skill of the character
        'Outputs: None
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Very simple function that opens the filestream, writer class and saves the attributes
        'Could extend the attributes to be global variables
        Dim FilePath As New FileStream(CharFilename, FileMode.Create, FileAccess.ReadWrite)

        Dim FileWriter As New StreamWriter(FilePath)

        FileWriter.BaseStream.Seek(0, SeekOrigin.End)

        FileWriter.WriteLine(MainCharacter.Name)
        FileWriter.WriteLine(MainCharacter.Strength)
        FileWriter.WriteLine(MainCharacter.Skill)

        FileWriter.Close()

    End Sub

    Function LoadCharacter(ByVal MainCharacter)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name : LoadCharacter
        'Description: Loads the characters details from a file. as a standard game only has one player
        'could be used to pick a random monster from a file
        'Inputs: none
        'Outputs: to a global variable that stores all the main charicter attributes in an array
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CharacterArray(ArraySize) As String
        Dim Counter As Integer = 0
        Dim FilePath As New FileStream(CharFilename, FileMode.Open, FileAccess.ReadWrite)
        Dim FileReader As New StreamReader(FilePath)

        FileReader.BaseStream.Seek(0, SeekOrigin.Begin)
        While FileReader.Peek() > -1
            CharacterArray(Counter) = FileReader.ReadLine()
            Console.WriteLine(CharacterArray(Counter))
            Counter = Counter + 1
        End While

        MainCharacter.Name = CharacterArray(0)
        MainCharacter.Strength = CharacterArray(1)
        MainCharacter.Skill = CharacterArray(2)

        FileReader.Close()
        Return (MainCharacter)
    End Function

End Module

