CREATE TABLE [dbo].[Conversacion] (
    [IdConversacion]           INT           IDENTITY (1, 1) NOT NULL,
    [FechaHora]                DATETIME      NULL,
    [NombreInicioConversacion] VARCHAR (250) NULL,
    [IdUsuario]                INT           NULL,
    CONSTRAINT [PK_Conversacion] PRIMARY KEY CLUSTERED ([IdConversacion] ASC)
);

