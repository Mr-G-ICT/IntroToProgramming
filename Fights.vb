Imports System.IO
Module Fights

    Function Modifier(ByVal charattribute1, ByVal charattribute2)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Name : Modifier
        'Description : Develops a modifier for 2 charicters before a fight, this will be used to 
        'decide how many hit points/XP the person gains/loses
        'Inputs: Attribute for charicter 1, attribute for charicter 2
        'Outpus: A Strength modifier
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim difference As Single

        'depending on which charicter has the greater bonus, work out the difference
        If charattribute1 > charattribute2 Then
            difference = charattribute1 - charattribute2
        Else
            difference = charattribute2 - charattribute1
        End If

        'return the difference divided by 5 to give a modifier
        Return (difference / 5)



    End Function

    Function FightClub(ByVal name, ByVal strength1, ByVal strength2, ByVal skill1, ByVal skill2, ByVal MonsterName)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Name : FightClub
        ' description : this is a function to decide the outcome of an encounter
        ' Inputs: main charicter strength and skill
        ' Inputs: opponents strength and skill
        ' Outputs: strength and skill of the charicter after the encounter
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'start the randomise function
        Randomize()

        'initilalise variables
        Dim StrengthModifier As Single
        Dim SkillModifier As Single
        Dim SixSidedDice As New Random
        Dim CharOneDiceRoll As Integer
        Dim CharTwoDiceRoll As Integer
        Dim death As Boolean = False

        'generate the modifiers for the encounter using Modifier function
        StrengthModifier = Modifier(strength1, strength2)
        SkillModifier = Modifier(skill1, skill2)

        Console.WriteLine(strength1 & strength2 & StrengthModifier & SkillModifier)
        Console.ReadLine()


        'loop until one charicter dies.  Charicter2 could be changed to enemy in real game so that it
        ' makes more sense
        While death <> True
            CharOneDiceRoll = SixSidedDice.Next(1, 6)
            CharTwoDiceRoll = SixSidedDice.Next(1, 6)

            'depending on the dice roll whichever person scores the highest wins that round and adds
            'the modifier to their strength. could be modified to health.
            If CharOneDiceRoll > CharTwoDiceRoll Then
                Console.WriteLine(name & " wins")
                strength1 = strength1 + StrengthModifier
                skill1 = skill1 + SkillModifier

                strength2 = strength2 - StrengthModifier
                skill2 = skill2 - SkillModifier

            ElseIf CharTwoDiceRoll > CharOneDiceRoll Then
                Console.WriteLine(MonsterName & " wins")
                strength1 = strength1 - StrengthModifier
                skill1 = skill1 - SkillModifier

                strength2 = strength2 + StrengthModifier
                skill2 = skill2 + SkillModifier

            Else
                Console.WriteLine("draw")
            End If

            'determines if a charicter has dies, loop quit condition
            If strength1 <= 0 Then
                Console.WriteLine(name & " has died, you are dead")
                death = True
                Return (-1)
            ElseIf strength2 <= 0 Then
                Console.WriteLine(MonsterName & " has died")
                Return StrengthModifier
                death = True
            End If

            Console.WriteLine(name & "'s strength " & strength1 & " " & name & "'s skill " & skill1)
            Console.WriteLine("Monster " & strength2 & " Monster " & skill2)
            Console.ReadLine()
        End While
    End Function

End Module

