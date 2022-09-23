CREATE TABLE [dbo].[Cuestionario] (
    [IdCuestionario]      INT           IDENTITY (1, 1) NOT NULL,
    [IdLeccion]           INT           NOT NULL,
    [Nombre]              VARCHAR (150) NOT NULL,
    [Descripcion]         VARCHAR (250) NULL,
    [Peso]                INT           NULL,
    [Estado]              BIT           NOT NULL,
    [UsuarioCreacion]     INT           NOT NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioModificacion] INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    CONSTRAINT [PK_Cuestionario] PRIMARY KEY CLUSTERED ([IdCuestionario] ASC)
);





