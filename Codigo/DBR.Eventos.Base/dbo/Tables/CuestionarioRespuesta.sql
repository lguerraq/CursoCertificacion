CREATE TABLE [dbo].[CuestionarioRespuesta] (
    [IdCuestionarioRespuesta] INT      IDENTITY (1, 1) NOT NULL,
    [IdCuestionarioTomado]    INT      NOT NULL,
    [IdPregunta]              INT      NOT NULL,
    [IdRespuesta]             INT      NOT NULL,
    [Estado]                  BIT      NOT NULL,
    [UsuarioCreacion]         INT      NOT NULL,
    [FechaCreacion]           DATETIME NOT NULL,
    [UsuarioModificacion]     INT      NULL,
    [FechaModificacion]       DATETIME NULL,
    CONSTRAINT [PK_CuestionarioRespuesta] PRIMARY KEY CLUSTERED ([IdCuestionarioRespuesta] ASC),
    CONSTRAINT [FK_CuestionarioRespuesta_CuestionarioTomado] FOREIGN KEY ([IdCuestionarioTomado]) REFERENCES [dbo].[CuestionarioTomado] ([IdCuestionarioTomado])
);

