CREATE TABLE [dbo].[Universidad] (
    [IdUniversidad] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]        VARCHAR (200) NULL,
    [Estado]        BIT           NULL,
    CONSTRAINT [PK_Universidad] PRIMARY KEY CLUSTERED ([IdUniversidad] ASC)
);

