# God of Football - Mobile Board Game

**Technical Overview:**
"God of Football" is a mobile board game developed in Unity, designed to provide an immersive and entertaining soccer experience through dice-based gameplay. This technical description will delve into various aspects of the game's development, highlighting key features, technologies used, and the development journey.

## Core Gameplay Mechanics:

- **Dice-Based Simulation:** The fundamental gameplay of "God of Football" revolves around the use of dice to simulate various soccer actions, including passing, shooting, fouling, and more. Each dice roll determines the outcome of these actions, adding an element of chance and strategy to the game.

![Gameplay](https://github.com/iFralex/GodOfFootball/assets/61825057/bace0b64-b49c-414a-9131-96d64ac57613)

- **Multiplayer and Singleplayer AI Capability:** The game allows players to engage in real-time multiplayer matches with other players through online connectivity. Additionally, players can opt for single-player matches against a sophisticated AI opponent developed specifically for the game.

- **Soccer Actions:** "God of Football" replicates real soccer actions, such as dribbling, crossing, and even disciplinary actions like issuing yellow cards and red cards. The outcome of each of these actions is determined by dice rolls, with probabilities taken into account.

![Gameplay](https://github.com/iFralex/GodOfFootball/assets/61825057/c406a214-6cc1-4773-a2b7-da4f07a01490)

- **Team Selection:** Players have access to a diverse selection of teams from various nations. Each team is characterized by its unique jersey design and often features a customized team chant, adding to the game's immersion.

![National teams choice](https://github.com/iFralex/GodOfFootball/assets/61825057/27d52238-5c85-4dc8-8098-1220cda5263d)

- **Score Feature**: The game has a scoring system that rewards winning players. The score behaves differently if you play in multiplayer or against AI.

- **Language Support:** The game is available in both English and Italian, catering to a broader player base.

## Development Technologies:

- **Unity Engine:** The game is developed using the [Unity](https://unity.com) game engine, which provides a robust environment for creating 2D and 3D games across various platforms.

- **C# Programming:** Core game logic and mechanics are implemented in C#, a programming language commonly used for Unity game development.

- **Multiplayer Integration:** The multiplayer functionality is implemented using [PUN 2](https://www.photonengine.com/pun) (Photon Unity Networking 2). PUN 2 facilitates real-time online interactions, enabling seamless multiplayer experiences.

- **Singleplayer AI:** The artificial intelligence that allows you to play in singleplayer was entirely developed and managed by me. He chooses the best options and calculates the possible options he can make.

- **Monetization Strategy:** [AdMob](https://admob.google.com/home/), Google's mobile advertising platform, is employed to generate revenue through advertisements strategically placed within the game.

- **Animations:** The game is full of curated animations, from the menu to the game. Each animation has been designed and created by hand frame by frame.

![Menu animation](https://github.com/iFralex/GodOfFootball/assets/61825057/c9e8f698-0d26-4dae-b47c-c40eac52a43b)


- **Development Timeline:** The project's inception dates back to early 2021 when it was commissioned by a passionate client. Six months of dedicated development, bug fixing, and fine-tuning were required to bring the game to Google Play.

## Technical Challenges and Improvements:

- **Experience Gained:** God of Football represents one of the my larger and more intricate projects. The experience gained from its development has been invaluable and contributed to the continuous growth of my skills and expertise in game development.

- **Too Large Size:** During the publication on Google PlayStore, I realized that it was too heavy. I proposed various solutions to the client, including implementing [Asset Bundles](https://docs.unity3d.com/Manual/AssetBundlesIntro.html), compacting all the graphics in a series of [atlas](https://docs.unity3d.com/Manual/class-SpriteAtlas.html) , and removing 2/3 of the teams. The client chose the last solution, so now there are hundreds of teams, but significantly fewer than the original. The other 2 most interesting solutions I have exploited in my biggest project: Bebe.

- **Custom Trailer:** The project also involved the creation of a promotional trailer, a testament to the comprehensive nature of the game development effort. I created the trailer via Final Cut Pro, and it's visible on Youtube at this [link](https://www.youtube.com/watch?v=bteuARJen7Y).

![Trailer](https://github.com/iFralex/GodOfFootball/assets/61825057/d182a85a-93a7-46f0-ae5b-bce65d7ea902)


In summary, "God of Football" is a mobile board game that successfully simulates soccer through dice-based gameplay. Developed in Unity, it includes advanced multiplayer features, a sophisticated AI opponent, real soccer actions, and extensive language support. The project's journey, challenges, and the use of various technologies have contributed to its complex and rewarding nature, making it one of the developer's notable accomplishments in the world of game development.
