CREATE TABLE [dbo].[Inscripcion] (
    [IdInscripcion]       INT              IDENTITY (1, 1) NOT NULL,
    [IdEvento]            INT              NULL,
    [IdPersona]           INT              NULL,
    [EstadoPago]          INT              NULL,
    [TipoPago]            INT              NULL,
    [EntregaCertificado]  BIT              NULL,
    [TipoModalidad]       INT              NULL,
    [Monto]               DECIMAL (9, 2)   NULL,
    [NombreBanco]         VARCHAR (250)    NULL,
    [FechaOperacion]      DATETIME         NULL,
    [NumeroOperacion]     VARCHAR (50)     NULL,
    [NumeroCertificado]   INT              NULL,
    [Certificado]         VARCHAR (150)    NULL,
    [Nota]                DECIMAL (9, 2)   NULL,
    [TipoInscripcion]     INT              NULL,
    [Ruc]                 VARCHAR (20)     NULL,
    [Estado]              BIT              NOT NULL,
    [UsuarioCreacion]     INT              NOT NULL,
    [FechaCreacion]       DATETIME         NOT NULL,
    [UsuarioModificacion] INT              NULL,
    [FechaModificacion]   DATETIME         NULL,
    [rowguid]             UNIQUEIDENTIFIER ROWGUIDCOL NULL,
    CONSTRAINT [PK_Inscripcion] PRIMARY KEY CLUSTERED ([IdInscripcion] ASC),
    CONSTRAINT [FK_Inscripcion_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([IdEvento]),
    CONSTRAINT [FK_Inscripcion_Persona] FOREIGN KEY ([IdPersona]) REFERENCES [dbo].[Persona] ([IdPersona])
);







