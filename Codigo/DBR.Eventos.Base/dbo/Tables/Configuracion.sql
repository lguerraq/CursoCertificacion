CREATE TABLE [dbo].[Configuracion] (
    [IdConfiguracion] INT          IDENTITY (1, 1) NOT NULL,
    [Variable]        VARCHAR (50) NOT NULL,
    [Valor]           VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Configuracion] PRIMARY KEY CLUSTERED ([IdConfiguracion] ASC)
);

