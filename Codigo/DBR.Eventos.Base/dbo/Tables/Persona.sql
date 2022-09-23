CREATE TABLE [dbo].[Persona] (
    [IdPersona]            INT           IDENTITY (1, 1) NOT NULL,
    [Nombres]              VARCHAR (50)  NULL,
    [ApellidoPaterno]      VARCHAR (50)  NULL,
    [ApellidoMaterno]      VARCHAR (50)  NULL,
    [NumeroDocumento]      VARCHAR (11)  NULL,
    [CIP]                  VARCHAR (50)  NULL,
    [Celular]              VARCHAR (20)  NULL,
    [Correo]               VARCHAR (50)  NULL,
    [Estado]               BIT           NOT NULL,
    [UsuarioCreacion]      INT           NOT NULL,
    [FechaCreacion]        DATETIME      NOT NULL,
    [UsuarioModificacion]  INT           NULL,
    [FechaModificacion]    DATETIME      NULL,
    [IdProfesion]          INT           NULL,
    [TipoOcupacion]        INT           NULL,
    [DescripcionOcupacion] VARCHAR (150) NULL,
    [IdPais]               INT           NULL,
    [Ciudad]               VARCHAR (200) NULL,
    CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED ([IdPersona] ASC),
    CONSTRAINT [FK_Persona_Pais] FOREIGN KEY ([IdPais]) REFERENCES [dbo].[Pais] ([IdPais]),
    CONSTRAINT [FK_Persona_Profesion] FOREIGN KEY ([IdProfesion]) REFERENCES [dbo].[Profesion] ([IdProfesion])
);



