
IF OBJECT_ID(N'dbo.Drivers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Drivers
    (
        Id int NOT NULL CONSTRAINT PK_Drivers PRIMARY KEY,
        Name nvarchar(max) NOT NULL,
        Team nvarchar(max) NOT NULL,
        Points int NOT NULL,
        CONSTRAINT CK_Drivers_Id CHECK (Id > 0),
        CONSTRAINT CK_Drivers_Name CHECK (LEN(LTRIM(RTRIM(Name))) > 0),
        CONSTRAINT CK_Drivers_Team CHECK (LEN(LTRIM(RTRIM(Team))) > 0),
        CONSTRAINT CK_Drivers_Points CHECK (Points >= 0)
    );
    INSERT INTO dbo.Drivers (Id, Name, Team, Points) VALUES
    (1, N'Кими Антонелли', N'Mercedes', 302),
    (2, N'Джордж Расселл', N'Mercedes', 236),
    (3, N'Льюис Хэмилтон', N'Ferrari', 199),
    (4, N'Ландо Норрис', N'McLaren', 186),
    (5, N'Шарль Леклер', N'Ferrari', 179),
    (6, N'Макс Ферстаппен', N'Red Bull Racing', 163),
    (7, N'Оскар Пиастри', N'McLaren', 120),
    (8, N'Исак Хаджар', N'Red Bull Racing', 86),
    (9, N'Лиам Лоусон', N'Racing Bulls', 59),
    (10, N'Пьер Гасли', N'Alpine', 41),
    (11, N'Арвид Линдблад', N'Racing Bulls', 37),
    (12, N'Франко Колапинто', N'Alpine', 27),
    (13, N'Оливер Берман', N'Haas F1 Team', 20),
    (14, N'Габриэль Бортолето', N'Audi', 10),
    (15, N'Нико Хюлькенберг', N'Audi', 7),
    (16, N'Эстебан Окон', N'Haas F1 Team', 7),
    (17, N'Карлос Сайнс', N'Williams', 7),
    (18, N'Александер Албон', N'Williams', 5),
    (19, N'Фернандо Алонсо', N'Aston Martin', 3),
    (20, N'Юки Цунода', N'Racing Bulls', 1),
    (21, N'Лэнс Стролл', N'Aston Martin', 0),
    (22, N'Валттери Боттас', N'Cadillac', 0),
    (23, N'Серхио Перес', N'Cadillac', 0);
END;

IF OBJECT_ID(N'dbo.Races', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Races
    (
        Id int NOT NULL CONSTRAINT PK_Races PRIMARY KEY,
        Name nvarchar(max) NOT NULL,
        Country nvarchar(max) NOT NULL,
        Laps int NOT NULL,
        CONSTRAINT CK_Races_Id CHECK (Id > 0),
        CONSTRAINT CK_Races_Name CHECK (LEN(LTRIM(RTRIM(Name))) > 0),
        CONSTRAINT CK_Races_Country CHECK (LEN(LTRIM(RTRIM(Country))) > 0),
        CONSTRAINT CK_Races_Laps CHECK (Laps > 0)
    );
END;
