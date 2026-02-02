INCLUDE globals.ink

-> startKnot

=== startKnot ===
Angel: I'm messenger of the All Father.
Angel: Be not afraid!
~spawnThoughts("fear")
Me: AAAAHHHHH!!!!
Me: ...
Me: Hello, Angel!

~spawnThoughts("memory, love, hate")
Memory, Hate, Love...
+ [memory: I remember...something] -> memory
+ [love: I love someone] -> love
+ [hate: Get lost!] -> hate
+ [beloved: It was all for you] -> beloved
+ [xenophobia: You are so odd] -> xenophobia
+ [nightmare: So afraid to sleep] -> nightmare
+ [ambivalence: A strange contradiction] -> ambivalence
+ [rejection: How dare you?] -> rejection

=== memory ===
Such a distant echo.
The cries of a suppressed mind.
~spawnThoughts("nightmare")
A forgotten revelation from my past self.
-> END

=== hate ===
I don't need help from smug do-gooders!
~spawnThoughts("rejection")
I'll find my own way out of here.
-> END

=== love ===
Totally not you!
~spawnThoughts("rejection")
-> END

=== beloved ===
My passion, every moment of my insignificant life led me...
~spawnThoughts("love, love, love")
...to you.
-> END

=== xenophobia ===
We do not welcome your kind here.
~spawnThoughts("fear")
-> END

=== nightmare ===
It's waiting for me.
~spawnThoughts("fear")
It will take me when I close my eyes.
-> END

=== ambivalence ===
A clash of emotions from which no one will emerge victorious.
-> END

=== rejection ===
For the rest of your life, you will be haunted by the torment of the happiness you missed.
~spawnThoughts("haunted, happiness")
Our shared happiness.
-> END