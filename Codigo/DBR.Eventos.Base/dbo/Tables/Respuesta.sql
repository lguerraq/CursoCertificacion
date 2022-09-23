CREATE TABLE [dbo].[Respuesta] (
    [IdRespuesta]         INT           IDENTITY (1, 1) NOT NULL,
    [IdPregunta]          INT           NOT NULL,
    [Descripcion]         VARCHAR (250) NOT NULL,
    [EsCorrecta]          BIT           NOT NULL,
    [Estado]              BIT           NOT NULL,
    [UsuarioCreacion]     INT           NOT NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioModificacion] INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    CONSTRAINT [PK_Respuesta] PRIMARY KEY CLUSTERED ([IdRespuesta] ASC),
    CONSTRAINT [FK_Respuesta_Pregunta] FOREIGN KEY ([IdPregunta]) REFERENCES [dbo].[Pregunta] ([IdPregunta])
);



