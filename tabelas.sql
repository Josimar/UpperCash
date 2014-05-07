CREATE DATABASE [UpperCash]
GO

USE [UpperCash]
GO

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'categoria')
BEGIN
	CREATE TABLE [dbo].[categoria](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[idpai] [int] NOT NULL,
		[nome] [varchar](128) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE dbo.categoria ADD CONSTRAINT FK_categoria_to_categoria
		FOREIGN KEY (idpai)
			REFERENCES dbo.categoria (id)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'conta')
BEGIN
	CREATE TABLE [dbo].[conta](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[valordisponivel] [float] NOT NULL,
		[nome] [varchar](128) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'pagamento')
BEGIN
	CREATE TABLE [dbo].[pagamento](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[nome] [varchar](128) NOT NULL,
		[limite] [float] NULL,
		[vencimento] [datetime] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'tipo')
BEGIN
	CREATE TABLE [dbo].[tipo](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[nome] [varchar](128) NOT NULL,
		[positivo] [bit] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'usuario')
BEGIN
	CREATE TABLE [dbo].[usuario](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[nome] [varchar](128) NULL,
		[email] [varchar](128) NULL,
		[login] [varchar](32) NULL,
		[senha] [varchar](32) NULL,
		[ativo] [bit] NULL,
		[idperfil] [int] NULL,
	 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'usuariocategoria')
BEGIN
	CREATE TABLE [dbo].[usuariocategoria](
		[idusuario] [int] NOT NULL,
		[idcategoria] [int] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE dbo.usuariocategoria ADD CONSTRAINT PK_usuariocategoria PRIMARY KEY CLUSTERED (idusuario, idcategoria) ON [PRIMARY]

	ALTER TABLE dbo.usuariocategoria ADD CONSTRAINT FK_usuariocategoria_to_usuario
		FOREIGN KEY (idusuario)
			REFERENCES dbo.usuario (id)

	ALTER TABLE dbo.usuariocategoria ADD CONSTRAINT FK_usuariocategoria_to_categoria
		FOREIGN KEY (idcategoria)
			REFERENCES dbo.categoria (id)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'usuariotipo')
BEGIN
	CREATE TABLE [dbo].[usuariotipo](
		[idusuario] [int] NOT NULL,
		[idtipo] [int] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE dbo.usuariotipo ADD CONSTRAINT PK_usuariotipo PRIMARY KEY CLUSTERED (idusuario, idtipo) ON [PRIMARY]

	ALTER TABLE dbo.usuariotipo ADD CONSTRAINT FK_usuariotipo_to_usuario
		FOREIGN KEY (idusuario)
			REFERENCES dbo.usuario (id)

	ALTER TABLE dbo.usuariotipo ADD CONSTRAINT FK_usuariotipo_to_tipo
		FOREIGN KEY (idtipo)
			REFERENCES dbo.tipo (id)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'usuarioconta')
BEGIN
	CREATE TABLE [dbo].[usuarioconta](
		[idusuario] [int] NOT NULL,
		[idconta] [int] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE dbo.usuarioconta ADD CONSTRAINT PK_usuarioconta PRIMARY KEY CLUSTERED (idusuario, idconta) ON [PRIMARY]

	ALTER TABLE dbo.usuarioconta ADD CONSTRAINT FK_usuarioconta_to_usuario
		FOREIGN KEY (idusuario)
			REFERENCES dbo.usuario (id)

	ALTER TABLE dbo.usuarioconta ADD CONSTRAINT FK_usuarioconta_to_conta
		FOREIGN KEY (idconta)
			REFERENCES dbo.conta (id)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'usuariopagamento')
BEGIN
	CREATE TABLE [dbo].[usuariopagamento](
		[idusuario] [int] NOT NULL,
		[idpagamento] [int] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE dbo.usuariopagamento ADD CONSTRAINT PK_usuariopagamento PRIMARY KEY CLUSTERED (idusuario, idpagamento) ON [PRIMARY]

	ALTER TABLE dbo.usuariopagamento ADD CONSTRAINT FK_usuariopagamento_to_usuario
		FOREIGN KEY (idusuario)
			REFERENCES dbo.usuario (id)

	ALTER TABLE dbo.usuariopagamento ADD CONSTRAINT FK_usuarioconta_to_pagamento
		FOREIGN KEY (idpagamento)
			REFERENCES dbo.pagamento (id)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'controle')
BEGIN
	CREATE TABLE [dbo].[controle](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[titulo] [varchar](128) NOT NULL,
		[descricao] [varchar](128) NULL,
		[observacao] [varchar](800) NULL,
		[valor] [float] NULL,
		[data] [datetime] NULL,
		[dataagendada] [datetime] NULL,
		[datapagamento] [datetime] NULL,
		[idcategoria] [int] NULL,
		[idtipo] [int] NULL,
		[idpagamento] [int] NULL,
		[idusuario] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE dbo.controle ADD CONSTRAINT FK_controle_to_usuario FOREIGN KEY (idusuario) REFERENCES dbo.usuario (id)
	ALTER TABLE dbo.controle ADD CONSTRAINT FK_controle_to_categoria FOREIGN KEY (idcategoria) REFERENCES dbo.categoria (id)
	ALTER TABLE dbo.controle ADD CONSTRAINT FK_controle_to_tipo FOREIGN KEY (idtipo) REFERENCES dbo.tipo (id)
	ALTER TABLE dbo.controle ADD CONSTRAINT FK_controle_to_pagamento FOREIGN KEY (idpagamento) REFERENCES dbo.pagamento (id)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sysobjects WHERE xtype = 'U' AND name = 'perfil')
BEGIN
	CREATE TABLE [dbo].[perfil](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[nome] [varchar](128) NOT NULL,
		[descricao] [varchar](128) NULL,
		[flags] [int] NULL
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT INTO Perfil(nome, descricao, flags) VALUES('Admin', 'Administrador', 0)
	INSERT INTO Perfil(nome, descricao, flags) VALUES('User', 'Usuário', 0)

	UPDATE Usuario SET idperfil = 1 

	ALTER TABLE dbo.usuario ADD CONSTRAINT FK_perfil_to_usuario FOREIGN KEY (idperfil) REFERENCES dbo.perfil (id)
END