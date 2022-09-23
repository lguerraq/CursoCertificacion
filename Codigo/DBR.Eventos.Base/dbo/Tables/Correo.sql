CREATE TABLE [dbo].[Correo] (
    [IdCorreo]            INT            IDENTITY (1, 1) NOT NULL,
    [Asunto]              VARCHAR (150)  NULL,
    [Origen]              VARCHAR (50)   NULL,
    [NombreOrigen]        VARCHAR (150)  NULL,
    [Mensaje]             NVARCHAR (MAX) NULL,
    [EstadoCorreo]        INT            NULL,
    [FechaEnvio]          DATETIME       NULL,
    [NumeroEnvio]         INT            NULL,
    [Estado]              BIT            CONSTRAINT [DF__Correo__Estado__4CA06362] DEFAULT ((1)) NOT NULL,
    [UsuarioCreacion]     INT            CONSTRAINT [DF__Correo__UsuarioC__4D94879B] DEFAULT ((1)) NOT NULL,
    [FechaCreacion]       DATETIME       CONSTRAINT [DF__Correo__FechaCre__4E88ABD4] DEFAULT (getdate()) NOT NULL,
    [UsuarioModificacion] INT            NULL,
    [FechaModificacion]   DATETIME       NULL,
    CONSTRAINT [PK_Correo] PRIMARY KEY CLUSTERED ([IdCorreo] ASC)
);



