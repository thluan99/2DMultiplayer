-> start

=== start ===
Hi, I am Adam, Can I help you ? #speaker:Adam #avatar:Adam
Hello, I am David. #speaker:David #avatar:David
I have many type of weapon. What do you want ? #speaker:Adam #avatar:Adam
 + [Gun] -> gun
 + [Sword] -> sword
    
=== gun ===
Some gun type!
 + [AK-47] -> chosen("AK-47")
 + [Thompson] -> chosen("Thompson")
 + [AWP] -> chosen("AWP")
-> END

=== sword ===
Some sword type!
 + [Caliburn] -> chosen("Caliburn")
 + [Muramasa] -> chosen("Muramasa")
 + [Old Sword] -> chosen("Old Sword")
-> END

=== chosen(type) ===
You choose {type}. Good luck!
-> END