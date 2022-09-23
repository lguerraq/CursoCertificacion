CREATE TABLE [dbo].[Pregunta] (
    [IdPregunta]          INT           IDENTITY (1, 1) NOT NULL,
    [IdCuestionario]      INT           NOT NULL,
    [Tipo]                INT           NOT NULL,
    [Nombre]              VARCHAR (250) NOT NULL,
    [Explicacion]         VARCHAR (500) NULL,
    [Ayuda]               VARCHAR (250) NULL,
    [Puntaje]             INT           NOT NULL,
    [Estado]              BIT           NOT NULL,
    [UsuarioCreacion]     INT           NOT NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioModificacion] INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    CONSTRAINT [PK_Pregunta] PRIMARY KEY CLUSTERED ([IdPregunta] ASC),
    CONSTRAINT [FK_Pregunta_Cuestionario] FOREIGN KEY ([IdCuestionario]) REFERENCES [dbo].[Cuestionario] ([IdCuestionario])
);





