CREATE TABLE [dbo].[CuestionarioTomado] (
    [IdCuestionarioTomado] INT             IDENTITY (1, 1) NOT NULL,
    [IdUsuario]            INT             NOT NULL,
    [IdCuestionario]       INT             NOT NULL,
    [Nota]                 DECIMAL (12, 2) NOT NULL,
    [Intento]              INT             NOT NULL,
    [Estado]               BIT             NOT NULL,
    [UsuarioCreacion]      INT             NOT NULL,
    [FechaCreacion]        DATETIME        NOT NULL,
    [UsuarioModificacion]  INT             NULL,
    [FechaModificacion]    DATETIME        NULL,
    CONSTRAINT [PK_CuestionarioTomado] PRIMARY KEY CLUSTERED ([IdCuestionarioTomado] ASC),
    CONSTRAINT [FK_CuestionarioTomado_Cuestionario] FOREIGN KEY ([IdCuestionario]) REFERENCES [dbo].[Cuestionario] ([IdCuestionario])
);





