## Presentation
Tevi is a mobile video game for natural awareness inspired by the Eden Project. 
It is being created by the MetaMakers Institute of Falmouth University’s Games Academy in partnership with the Eden Project. 
As a part of MetaMakers Institute research project, it uses interactive evolutionary computation to simulate a garden. 
The video game Tevi is also an experiment on using video games for natural heritage valorisation.

## Acknowledgment
Please copy this if you reuse part or the totality of this project:
>The code used here is notably based on the experimental videogame Tevi, created by the MetaMakers Institute of Falmouth University. 
>Project funded by the European Commission ERA Chair grant; an EPSRC Leadership Fellowship; and an EPSRC Next Step Digital Economy Hub grant.
>Code written by: Edwige Lelievre, Giovanni Rubino and Tim Philipps.
>Created in partnership with the Eden Project.

## Requirements
What you will need to make Tevi fully work for you:
-	Unity (free download, we used the version 2018.7.7f1)
-	An online MYSQL database (5.7.25)
-	An online server running PHP with writing rights (5.6.39) 
-	A Pl@ntnet API key (the API for plant recognition. They give free test key, but you will be able to only scan 100 images a day. Contact them on their website: https://plantnet.org/)
-	A webcam on your test computer so you can test plant recognition
-	A plant (can be a leaf)
If you want Pl@antnet API to work, you need online database and server so you can send them the url of your uploaded images.

## Step by step guide
1/ Download the project. You can delete (or move) the “xx_Php_mysql” folder (in the "Assets" folder) where the .php and .sql files are stored as they are not needed in Unity. Open the project.
Then create a “HIDDENdata.cs” script in which you will put your server and API information:

```
using System.Collections;

public static class HIDDENdata {
    public static bool exists = true;
    public static string website = "yourserveraddress";
    public static string hash = "yourhash"; 
    public static string apikey = "yourkeyhere"; 
}
```

2/ Download “metamakers_db.sql”. In your database, import metamakers_db.sql (the tables are empty except the one with the quests). Make sure you can write on it and that empty values are allowed to be written (if not, check mysqlmode of your server).
3/ On your server create a folder called “kreskanta”. Inside Create a folder called “images_upload”. You should have full writing rights on this folder as Unity+php will upload pictures there.
Fill the file “config_inc.php” with your database information and you hashcode (can be anything like “freetevi01” but it has to be the same as in Unity)
Put the .php files on your server in “kreskanta” folder. 
4/ Play!
Run the scene “main_menu” in Unity. You can get warnings but no errors. Register a new account. 
Play and follow the tutorial. 
In the menu on top right, open the camera, it should display what your webcam sees. Click on the blue button at the bottom when you have a plant in front of your camera. It should tell you what plant it is and give you a reward. 
Well done, you did it!

----------
## Contacts
edwige.lelievre@gmail.com @edwigelel on Twitter
rubinogiov@gmail.com @GioBorrows on Twitter
timphillipsgames@gmail.com @tpcphillips on Twitter

## Requirements without plant recognition
What you will need to make Tevi work without plant recognition:
-	Unity (free download, we used the version 2018.7.7f1)
-	An MYSQL database (5.7.25) : can be local
-	An server running PHP with writing rights (5.6.39) : can be local


## Research and development team
Dr. Edwige Lelièvre, MetaMakers Institute Research Fellow: Team leader, game design, programming, graphic design
Giovanni Rubino, MetaMakers Institute Game designer: Game design, programming, playtesting, graphic design
Tim Phillips, MetaMakers Institute Game designer: Game design, programming, playtesting
Dr. Joan Casas Roma MetaMakers Institute Research Fellow: Music, game design
Blanca Perez Ferrer, MetaMakers Institute Curator: Graphic design, playtesting, game design
Dr. Rob Saunders, Director of the MetaMakers Institute: Scientific supervision

## Advisors at Eden
Dr. Jo Elworthy, director of Interpretation
Chris Bisson, policy development manager, horticulturist
Catherine Cutler, biome supervisor, horticulturist
Céline Holman, senior exhibit designer
John Porter, biome supervisor, horticulturist
Chris Jenord
Lucy Wenger
Gabriela Gilkes

## Advisors at Falmouth University
Dr. Pr. Tanya Krzywinska
Ché Wilbraham
Dr. Edward Powley
Brian McDonald
Heidi Ball
Terry Greer
Dr. Rory Summerley

## External contractors
Searra Dodds: UI art
Pheobe Herring: artworks
Troy Atkinson: Ios port
Maddalena Gattoni: trailer
Neal Megaw and Zach Ellison of MAYN Creative: filming for user research
Florïn Zolli: flyer design and 2D illustration

The analysis of plants’ pictures is made using Pl@ntnet API.
