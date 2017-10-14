CREATE DATABASE MediaDB
GO

USE MediaDB
GO

CREATE TABLE [dbo].[Movie](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Title] [VARCHAR](50) NOT NULL,
	[Director] [VARCHAR](50) NOT NULL,
	[File] [VARBINARY](max) NULL,
 CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[Song](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Title] [VARCHAR](50) NOT NULL,
	[Singer] [VARCHAR](50) NOT NULL,
	[File] [VARBINARY](max) NULL,
 CONSTRAINT [PK_Song] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE PROC [dbo].[spAddMovie] (@Title VARCHAR(50), @Director VARCHAR(50), @File VARBINARY(max) = NULL)
AS
IF(@File is NULL)
	INSERT INTO dbo.Movie (Title, Director) VALUES (@Title, @Director);
ELSE
	INSERT INTO dbo.Movie (Title, Director, [File]) VALUES (@Title, @Director, @File);
RETURN SELECT SCOPE_IDENTITY();
GO

CREATE PROC [dbo].[spAddSong] (@Title VARCHAR(50), @Singer VARCHAR(50), @File VARBINARY(max) = NULL)
AS
IF(@File is NULL)
	INSERT INTO dbo.Song (Title, Singer) VALUES (@Title, @Singer);
ELSE
	INSERT INTO dbo.Song (Title, Singer, [File]) VALUES (@Title, @Singer, @File);
RETURN SELECT SCOPE_IDENTITY();
GO

CREATE PROC [dbo].[spDeleteMovie] (@Id INT)
AS
DELETE FROM dbo.Movie WHERE Id = @Id
GO

CREATE PROC [dbo].[spDeleteSong] (@Id INT)
AS
DELETE FROM dbo.Song WHERE Id = @Id
GO

CREATE PROC [dbo].[spUpdateMovie] (@Id INT, @Title VARCHAR(50), @Director VARCHAR(50), @File VARBINARY(max) = NULL)
AS
UPDATE dbo.Movie SET Title = @Title, Director = @Director, [File] = @File WHERE Id = @Id
GO

CREATE PROC [dbo].[spUpdateSong] (@Id INT, @Title VARCHAR(50), @Singer VARCHAR(50), @File VARBINARY(max) = NULL)
AS
UPDATE dbo.Song SET Title = @Title, Singer = @Singer, [File] = @File WHERE Id = @Id
GO