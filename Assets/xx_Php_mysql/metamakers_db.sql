-- phpMyAdmin SQL Dump
-- version 4.6.6deb5
-- https://www.phpmyadmin.net/
--
-- Client :  localhost:3306
-- Généré le :  Ven 12 Avril 2019 à 10:55
-- Version du serveur :  5.7.25-0ubuntu0.18.04.2
-- Version de PHP :  5.6.39-1+ubuntu18.04.1+deb.sury.org+1


--
-- Base de données :  `metamakers_db`
--

-- --------------------------------------------------------

--
-- Structure de la table `kr_inventory`
--

CREATE TABLE `kr_inventory` (
  `id_inventory` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `itemName` varchar(50) NOT NULL,
  `itemCount` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Structure de la table `kr_plantnet`
--

CREATE TABLE `kr_plantnet` (
  `id_plant` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `image_name` varchar(70) NOT NULL,
  `plant0` varchar(70) NOT NULL,
  `common0` varchar(70) NOT NULL,
  `score0` float NOT NULL,
  `plant1` varchar(70) NOT NULL,
  `common1` varchar(70) NOT NULL,
  `score1` float NOT NULL,
  `plant2` varchar(70) NOT NULL,
  `common2` varchar(70) NOT NULL,
  `score2` float NOT NULL,
  `date` datetime NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Structure de la table `kr_quest_player`
--

CREATE TABLE `kr_quest_player` (
  `id_quest_player` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `id_quest` int(11) NOT NULL,
  `quest_completed` tinyint(1) NOT NULL,
  `begin_date` datetime NOT NULL,
  `completion_date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Structure de la table `kr_quest_templates`
--

CREATE TABLE `kr_quest_templates` (
  `id_quest` int(11) NOT NULL,
  `questLore` longtext NOT NULL,
  `kindOfQuest` int(11) NOT NULL,
  `objectiveItem` varchar(100) NOT NULL,
  `objectiveValue` int(11) NOT NULL,
  `rewardItem` varchar(100) NOT NULL,
  `rewardValue` int(11) NOT NULL,
  `multiplierXP` int(11) NOT NULL,
  `active` tinyint(1) NOT NULL,
  `tutorialQuestNumber` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Contenu de la table `kr_quest_templates`
--

INSERT INTO `kr_quest_templates` (`id_quest`, `questLore`, `kindOfQuest`, `objectiveItem`, `objectiveValue`, `rewardItem`, `rewardValue`, `multiplierXP`, `active`, `tutorialQuestNumber`) VALUES
(0, 'Welcome! Let\'s start by planting your first seed. You\'ve been provided with a Shampoo Ginger Seed. Tap on it and then on a soil tile to plant it.\\nThis is going to be the most eco-friendly garden of the planet!', 3, 'ShampooGingerSeed', -1, 'Inspector', 1, 2, 0, 0),
(1, 'Now let\'s see how your plant is doing. Tap on the inspector and on the plant you want to check out, to see what it needs.', 3, 'Inspector', -1, 'WateringCan', 1, 1, 0, 1),
(2, 'Looks like your plant needs a bit of water. Use the watering can to water it. \\nIf you\'ve run out of something, you can always use the PlantNet camera to take a picture of a real plant. You\'ll be rewarded with a random item.', 3, 'WateringCan', -1, 'Fertilizer', 1, 1, 0, 2),
(3, 'Now a bit of Fertilizer to make it the happiest ginger plant you\'ve ever seen. \\nAgain, if you\'ve run out, there\'s always the PlantNet camera.', 3, 'Fertilizer', -1, 'BananaSeed', 1, 1, 0, 3),
(4, 'Great, you got the basics. Time to put your plant to use. make it grow nice and healthy, and once it\'s time, collect the bananas it produces.\\nIf you run out of seeds, try using plantnet and take a picture of a plant.', 0, 'BananaFruit', 2, 'BreadfruitSeed', 1, 1, 0, 4),
(5, 'The people of Earth are hungry. They ask that you focus your garden on food production.', 1, 'food', 25, 'BananaSeed', 3, 1, 0, 0),
(6, 'Earthlings, in their naivety, are still reliant on combustion for energy. You are asked to focus your garden on fuel production.', 1, 'fuel', 25, 'BreadfruitSeed', 3, 1, 0, 0),
(7, 'Somehow Earth has found a way to keep expanding. They would like you to focus your garden on construction materials.', 1, 'construction', 25, 'SugarcaneSeed', 3, 1, 0, 0),
(8, 'Oh No! An unvaccinated toddler caused an outbreak on Earth! Please, focus your garden on medicine production.', 1, 'medicine', 25, 'ShampooGingerSeed', 3, 1, 0, 0),
(9, 'Life on Earth is hard. The people need distractions. Please, focus some of your garden on culture.', 1, 'culture', 25, 'WildindigoSeed', 3, 1, 0, 0),
(10, 'Mother Earth is calling. They\'re offering some valuable Fertilizer in exchange of a little bit of our produce.', 0, 'BananaFruit', 2, 'Fertilizer', 4, 1, 0, 0),
(11, 'Mother Earth is calling. They\'re offering some valuable Fertilizer in exchange of a little bit of our produce.', 0, 'BreadfruitFruit', 2, 'Fertilizer', 4, 1, 0, 0),
(12, 'A neighbouring settlement is advertising a surplus in water. They offer it to others following the cause, in exchange of some of our produce.', 0, 'ShampooGingerFruit', 2, 'WateringCan', 4, 1, 0, 0),
(13, 'A neighbouring settlement is advertising a surplus in water. They offer it to others following the cause, in exchange of some of our produce.', 0, 'SugarcaneFruit', 2, 'WateringCan', 4, 1, 0, 0),
(14, 'Mother Earth is calling. They\'re offering some valuable Fertilizer in exchange of a little bit of our produce.', 0, 'WildindigoFruit', 2, 'Fertilizer', 4, 1, 0, 0);

-- --------------------------------------------------------

--
-- Structure de la table `kr_soils`
--

CREATE TABLE `kr_soils` (
  `id_soil` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `soil_name` varchar(20) NOT NULL,
  `posX` int(11) NOT NULL,
  `posY` int(11) NOT NULL,
  `water_lvl` int(11) NOT NULL DEFAULT '10',
  `nutrient_lvl` int(11) NOT NULL DEFAULT '10',
  `is_planted` tinyint(1) DEFAULT '0',
  `has_babyplant` tinyint(1) DEFAULT NULL,
  `plant_name` varchar(30) DEFAULT NULL,
  `plant_time` float UNSIGNED DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Structure de la table `kr_users`
--

CREATE TABLE `kr_users` (
  `playerId` int(10) UNSIGNED NOT NULL,
  `playerUsername` varchar(100) CHARACTER SET utf8 NOT NULL,
  `playerEmail` varchar(230) CHARACTER SET utf8 NOT NULL,
  `playerPassword` varchar(50) CHARACTER SET utf8 NOT NULL,
  `playerLvl` int(11) NOT NULL DEFAULT '0',
  `playerCorp` int(11) NOT NULL,
  `playerNumbSoilx` int(11) NOT NULL,
  `playerNumbSoily` int(11) NOT NULL,
  `playerXP` int(11) NOT NULL,
  `playerAccountCreation` datetime NOT NULL,
  `playerLastConnexion` datetime NOT NULL,
  `playerLastShipment` datetime DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Index pour les tables exportées
--

--
-- Index pour la table `kr_inventory`
--
ALTER TABLE `kr_inventory`
  ADD PRIMARY KEY (`id_inventory`),
  ADD UNIQUE KEY `id_inventory` (`id_inventory`);

--
-- Index pour la table `kr_plantnet`
--
ALTER TABLE `kr_plantnet`
  ADD PRIMARY KEY (`id_plant`),
  ADD UNIQUE KEY `id_plant` (`id_plant`);

--
-- Index pour la table `kr_quest_player`
--
ALTER TABLE `kr_quest_player`
  ADD PRIMARY KEY (`id_quest_player`);

--
-- Index pour la table `kr_quest_templates`
--
ALTER TABLE `kr_quest_templates`
  ADD PRIMARY KEY (`id_quest`);

--
-- Index pour la table `kr_soils`
--
ALTER TABLE `kr_soils`
  ADD PRIMARY KEY (`id_soil`),
  ADD UNIQUE KEY `id_soil` (`id_soil`);

--
-- Index pour la table `kr_users`
--
ALTER TABLE `kr_users`
  ADD PRIMARY KEY (`playerId`);

--
-- AUTO_INCREMENT pour les tables exportées
--

--
-- AUTO_INCREMENT pour la table `kr_inventory`
--
ALTER TABLE `kr_inventory`
  MODIFY `id_inventory` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT pour la table `kr_plantnet`
--
ALTER TABLE `kr_plantnet`
  MODIFY `id_plant` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT pour la table `kr_quest_player`
--
ALTER TABLE `kr_quest_player`
  MODIFY `id_quest_player` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT pour la table `kr_quest_templates`
--
ALTER TABLE `kr_quest_templates`
  MODIFY `id_quest` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT pour la table `kr_soils`
--
ALTER TABLE `kr_soils`
  MODIFY `id_soil` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT pour la table `kr_users`
--
ALTER TABLE `kr_users`
  MODIFY `playerId` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

