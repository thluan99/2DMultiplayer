EXTERNAL playEmote(emoteName)

-> start

=== start ===
Hi, I am David, Can I help you ?
~ playEmote("exclaimation")
What do you need ?
+ [Tea] -> tea
+ [Coca] -> coca
+ [Coffee] -> coffee
    
=== tea ===
OK, I will give you a tea!
Do you want to try a Moc Chau tea.
~ playEmote("question")
 + [Yes] -> mocchau
 + [No] -> imocchau
-> END

=== mocchau ===
Okay, I will.
-> END
=== imocchau ===
Thanks,
Good bye!
-> END

=== coca ===
Oh, One coca!
-> END

=== coffee ===
Coffee!
-> END