using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LDToypad
{
    public enum Character
    {
        /// <summary>
        /// ¯\_(ツ)_/¯ No idea what this is
        /// </summary>
        Unrecognized = -1,

        /// <summary>
        /// We haven't worked out which character yet
        /// </summary>
        Unknown = 0,

        //! Characters start here

        Batman = 1,
        [Description("Gandalf the Grey")]
        Gandalf = 2,
        Wyldstyle = 3,
        Aquaman = 4,
        [Description("Bad Cop")]
        BadCop = 5,
        Bane = 6,
        Bart = 7,
        Benny = 8,
        Chell = 9,
        Cole = 10,
        Cragger = 11,
        Cyborg = 12,
        Cyberman = 13,
        [Description("Doc Brown")]
        DocBrown = 14,
        [Description("The Doctor")]
        Doctor = 15,
        Emmet = 16,
        Eris = 17,
        Gimli = 18,
        Smeagol = 19,
        [Description("Harley Quinn")]
        HarleyQuinn = 20,
        Homer = 21,
        Jay = 22,
        Joker = 23,
        Kai = 24,
        [Description("ACU Trooper")]
        ACU = 25,
        [Description("Gamer Kid")]
        GamerKid = 26,
        [Description("Krusty the Clown")]
        Krusty = 27,
        Laval = 28,
        Legolas = 29,
        Lloyd = 30,
        [Description("Marty McFly")]
        MartyMcfly = 31,
        Nya = 32,
        [Description("Owen Grady")]
        Owen = 33,
        [Description("Peter Venkman")]
        PeterVenkman = 34,
        Slimer = 35,
        [Description("Scooby Doo")]
        ScoobyDoo = 36,
        [Description("Sensei Wu")]
        SenseiWu = 37,
        Shaggy = 38,
        [Description("Stay Pyft")]
        StayPuft = 39,
        Superman = 40,
        Unikitty = 41,
        [Description("Wicked Witch of the West")]
        WickedWitch = 42,
        [Description("Wonder Woman")]
        WonderWoman = 43,
        Zane = 44,
        [Description("Green Arrow")]
        GreenArrow = 45,
        Supergirl = 46,
        [Description("Abby Yates")]
        AbbyYates = 47,
        Finn = 48,
        [Description("Ethan Hunt")]
        EthanHunt = 49,
        [Description("Lumpy Space Princess")]
        LumpySpacePrincess = 50,
        Jake = 51,
        [Description("Harry Potter")]
        HarryPotter = 52,
        [Description("Lord Voldemort")]
        Voldemort = 53,
        [Description("Michael Knight")]
        MichaelKnight = 54,
        [Description("B.A. Baracus")]
        BABaracus = 55,
        [Description("Newt Scamander")]
        Newt = 56,
        [Description("Sonic the Hedgehog")]
        Sonic = 57,
        Gizmo = 59,
        Stripe = 60,
        [Description("E.T.")]
        ET = 61,
        [Description("Tina Goldstein")]
        Tina = 62,
        [Description("Marceline the Vampire Queen")]
        Marceline = 63,
        [Description("")]
        Batgirl = 64,
        Robin = 65,
        Sloth = 66,
        [Description("Hermione Granger")]
        HermioneGranger = 67,
        [Description("Chase McCain")]
        ChaseMcCain = 68,
        [Description("Excalibur Batman")]
        ExcaliburBatman = 69,
        Raven = 70,
        [Description("Beast Boy")]
        BeastBoy = 71,
        Betelgeuse = 72,
        [Description("Lort Vortech")]
        LordVortech = 73,
        Blossom = 74,
        Bubbles = 75,
        Buttercup = 76,
        Starfire = 77,
        [Description("Supergirl (Red Lantern)")]
        SupergirlRedLantern = 776,

        //! Vehicles start here
        [Description("Police Car")]
        PoliceCar = 1000,
        [Description("Aerial Squad Car")]
        AerialSquadCar = 1001,
        [Description("Missile Striker")]
        MissileStriker = 1002,
        [Description("Gravity Sprinter")]
        GravitySprinter = 1003,
        [Description("Street Shredder")]
        StreetShredder = 1004,
        [Description("Sky Clobberer")]
        SkyClobberer = 1005,
        Batmobile = 1006,
        Batblaster = 1007,
        [Description("Sonic Batray")]
        SonicBatray = 1008,
        [Description("Benny's Spaceship")]
        BennysSpaceship = 1009,
        Lasercraft = 1010,
        [Description("The Annihilator")]
        TheAnnihilator = 1011,
        [Description("DeLorean Time Machine")]
        DeLorean = 1012,
        [Description("Ultra Time Machine")]
        UltraTimeMachine = 1013,
        [Description("Electric")]
        ElectricTimeMachine = 1014,
        Hoverboard = 1015,
        [Description("Cyclone Board")]
        CycloneBoard = 1016,
        [Description("Ultimate Hoverjet")]
        UltimateHoverjet = 1017,
        EagleInterceptor = 1018,
        EagleSkyblazer = 1019,
        EagleSwoopDiver = 1020,
        CraggersFireship = 1021,
        CrocCommandSub = 1022,
        SwampSkimmer = 1023,
        CyberGuard = 1024,
        CyberWrecker = 1025,
        LaserRobotWalker = 1026,
        K9 = 1027,
        K9RuffRover = 1028,
        K9LaserCutter = 1029,
        TARDIS = 1030,
        LaserPulseTARDIS = 1031,
        EnergyBurstTARDIS = 1032,
        EmmetsExcavator = 1033,
        TheDestroydozer = 1034,
        DestructOMech = 1035,
        WingedMonkey = 1036,
        BattleMonkey = 1037,
        CommanderMonkey = 1038,
        AxeChariot = 1039,
        AxeHurler = 1040,
        SoaringChariot = 1041,
        ShelobTheGreat = 1042,
        EightLeggedStalker = 1043,
        PoisonSlinger = 1044,
        HomersCar = 1045,
        Homercraft = 1046,
        SubmaHomer = 1047,
        TauntOVision = 1048,
        BlastCam = 1049,
        TheMechaHomer = 1050,
        Velociraptor = 1051,
        SpikeAttackRaptor = 1052,
        VenomRaptor = 1053,
        GyroSphere = 1054,
        SonicBeamGyrosphere = 1055,
        SpeedBoostGyrosphere = 1056,
        ClownBike = 1057,
        CannonBike = 1058,
        AntiGravityRocketBike = 1059,
        MightyLionRider = 1060,
        LionBlazer = 1061,
        FireLion = 1062,
        ArrowLauncher = 1063,
        SeekingShooter = 1064,
        TripleBallista = 1065,
        MysteryMachine = 1066,
        MysteryTow = 1067,
        MysteryMonster = 1068,
        BoulderBomber = 1069,
        BoulderBlaster = 1070,
        CycloneJet = 1071,
        StormFighter = 1072,
        LightningJet = 1073,
        ElectroShooter = 1074,
        BladeBike = 1075,
        FlyingFireBike = 1076,
        BladesOfFire = 1077,
        SamuraiMech = 1078,
        SamuraiShooter = 1079,
        SoaringSamuraiMech = 1080,
        CompanionCube = 1081,
        LaserDeflector = 1082,
        GoldHeartEmitter = 1083,
        SentryTurret = 1084,
        TurretStriker = 1085,
        FlyingTurretCarrier = 1086,
        ScoobySnack = 1087,
        ScoobyFireSnack = 1088,
        ScoobyGhostSnack = 1089,
        CloudCukkoCar = 1090,
        XStreamSoaker = 1091,
        RainbowCannon = 1092,
        InvisibleJet = 1093,
        StealthLaserShooter = 1094,
        TorpedoBomber = 1095,
        NinjaCopter = 1096,
        Glaciator = 1097,
        FreezeFighter = 1098,
        TravelingTimeTrain = 1099,
        TravelingTimeTrainRebuilt1 = 1100,
        TravelingTimeTrainRebuilt2 = 1101,
        AquaWatercraft = 1102,
        AquaWatercraftRebuilt1 = 1103,
        AquaWatercraftRebuilt2 = 1104,
        DrillDriver = 1105,
        DrillDriverRebuilt1 = 1106,
        DrillDriverRebuilt2 = 1107,
        QuinnMobile = 1108,
        QuinnMobileRebuilt1 = 1109,
        QuinnMobileRebuilt2 = 1110,
        TheJokersChopper = 1111,
        TheJokersChopperRebuilt1 = 1112,
        TheJokersChopperRebuilt2 = 1113,
        HoverPod = 1114,
        HoverPodRebuilt1 = 1115,
        HoverPodRebuilt2 = 1116,
        Dalek = 1117,
        DalekRebuilt1 = 1118,
        DalekRebuilt2 = 1119,
        Ecto1 = 1120,
        Ecto1Rebuilt1 = 1121,
        Ecto1Rebuilt2 = 1122,
        GhostTrap = 1123,
        GhostTrapRebuilt1 = 1124,
        GhostTrapRebuilt2 = 1125,
        LlyodsGoldenDragon = 1132,
        GoldenDragonRebuilt1 = 1133,
        GoldenDragonRebuilt2 = 1134,
        MegaFlightDragon = 1144,
        MegaFlightDragonRebuilt1 = 1145,
        MegaFlightDragonRebuilt2 = 1146,
        FlyingWhiteDragon = 1155,
        GoldenFireDragon = 1156,
        UltraDestructionDragon = 1157,
        ArcadeMachine = 1158,
        EightBitShooter = 1159,
        ThePixelator = 1160,
        G61555SpyHunter = 1161,
        TheInterdiver = 1162,
        AerialSpyhunter = 1163,
        SlimeShooter = 1164,
        SlimeExploder = 1165,
        SlimeStreamer = 1166,
        TerrorDog = 1167,
        TerrorDogDestroyer = 1168,
        SoaringTerrorDog = 1169,
        AncientWarElephant = 1170,
        CosmicSquid = 1171,
        PsychicSubmarine = 1172,
        BMO = 1173,
        DOGMO = 1174,
        SNAKEMO = 1175,
        Jakemoblie = 1176,
        SnailDudeJake = 1177,
        HoverJake = 1178,
        LumpyCar = 1179,
        LumpyTruck = 1180,
        LumpyLandWhale = 1181,
        LunaticAmp = 1182,
        LunaticAmp2 = 1183,
        LunaticAmp3 = 1184,
        BAsVan = 1185,
        FoolSmasher = 1186,
        ThePainPlane = 1187,
        PhoneHome = 1188,
        PhoneHome2 = 1189,
        PhoneHome3 = 1190,
        Niffler = 1191,
        Niffler2 = 1192,
        Niffler3 = 1193,
        SwoopingEvil = 1194,
        SwoopingEvil2 = 1195,
        SwoopingEvil3 = 1196,
        Ecto12016 = 1197,
        Ectozer = 1198,
        PerfEcto = 1199,
        FlashNFinish = 1200,
        RampageRecordPlayer = 1201,
        StripesThrone = 1202,
        RCRacer = 1203,
        GadetOMatic = 1204,
        ScarletScorpion = 1205,
        HogwartsExpress = 1206,
        SoaringSteamPlane = 1207,
        SteamWarrior = 1208,
        EnchantedCar = 1209,
        SharkSub = 1210,
        MonstrousMouth = 1211,
        IMFScrambler = 1212,
        ShockCycle = 1213,
        IMFCovertJet = 1214,
        IMFSportCar = 1215,
        IMFTank = 1216,
        IMFSplorer = 1217,
        SonicSpeedster = 1218,
        SonicSpeedster2 = 1219,
        SonicSpeedster3 = 1220,
        TheTornado = 1221,
        TheTornado1 = 1222,
        TheTornado2 = 1223,
        KITT = 1224,
        KITTJet = 1225,
        GoliathArmoredSemi = 1226,
        Police = 1227,
        Hovercraft = 1228,
        Plane = 1229,
        BionicSteed = 1230,
        BatRaptor = 1231,
        Ultrabat = 1232,
        BatWing = 1233,
        BlackThunder = 1234,
        BatTank = 1235,
        SkeletonOrgan = 1236,
        Jukebox = 1237,
        SkeleTurkey = 1238,
        PirateShip = 1239,
        FangedFortune = 1240,
        Inferno = 1241,
        Buckbeak = 1242,
        GiantOwl = 1243,
        FierceFalcon = 1244
    }
}
