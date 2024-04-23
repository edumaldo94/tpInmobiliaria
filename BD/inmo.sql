-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3306
-- Tiempo de generación: 23-04-2024 a las 11:23:36
-- Versión del servidor: 8.0.30
-- Versión de PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmo`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoria`
--

CREATE TABLE `auditoria` (
  `IdAuditor` int NOT NULL,
  `ContratoId` int DEFAULT NULL,
  `PagoId` int DEFAULT NULL,
  `UsuarioNombre` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `FechaHora` datetime DEFAULT CURRENT_TIMESTAMP,
  `Accion` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `auditoria`
--

INSERT INTO `auditoria` (`IdAuditor`, `ContratoId`, `PagoId`, `UsuarioNombre`, `FechaHora`, `Accion`) VALUES
(1, 60, 145, 'javi@gmail.com', '2024-04-23 00:20:32', 'Creación de Contrato y Pago'),
(2, 60, 146, 'javi@gmail.com', '2024-04-23 02:15:38', 'Fin de Contrato y Pago de multa'),
(3, 56, 147, 'javi@gmail.com', '2024-04-23 02:50:57', 'Abono'),
(4, 55, 125, 'javi@gmail.com', '2024-04-23 03:04:07', 'No Abono'),
(5, 55, 125, 'javi@gmail.com', '2024-04-23 03:06:04', 'Abono'),
(6, 56, 147, 'javi@gmail.com', '2024-04-23 04:40:19', 'No Abono'),
(7, 56, 147, 'javi@gmail.com', '2024-04-23 04:41:18', 'Abono'),
(8, 28, 148, 'nina@gmail.com', '2024-04-23 04:53:23', 'Abono'),
(10, 28, 69, 'javi@gmail.com', '2024-04-23 05:04:37', 'No Abono'),
(11, 28, 69, 'javi@gmail.com', '2024-04-23 05:04:56', 'Abono');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id_Contrato` int NOT NULL,
  `inmuebleId` int NOT NULL,
  `inquilinoId` int NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `monto` double(10,2) DEFAULT NULL,
  `estado` varchar(255) NOT NULL,
  `estadoC` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id_Contrato`, `inmuebleId`, `inquilinoId`, `fecha_inicio`, `fecha_fin`, `monto`, `estado`, `estadoC`) VALUES
(28, 8, 5, '2020-03-31', '2023-04-03', 10.00, 'Activo', 1),
(49, 20, 5, '2024-04-17', '2024-08-17', 120500.00, 'No Activo', 1),
(52, 20, 5, '2024-08-18', '2024-10-18', 145500.00, 'No Activo', 1),
(53, 20, 4, '2025-01-01', '2025-03-01', 145500.00, 'Activo', 1),
(54, 7, 4, '2024-04-27', '2024-07-27', 350268.74, 'Activo', 1),
(55, 21, 4, '2024-04-18', '2024-08-18', 350789.00, 'Activo', 1),
(56, 19, 5, '2024-04-19', '2024-08-19', 152300.00, 'Activo', 1),
(60, 14, 6, '2024-04-18', '2024-12-23', 145500.00, 'No Activo', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id_Inmuebles` int NOT NULL,
  `propietarioId` int NOT NULL,
  `latitud` double DEFAULT NULL,
  `longitud` double DEFAULT NULL,
  `ubicacion` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci DEFAULT NULL,
  `direccion` varchar(100) DEFAULT NULL,
  `ambientes` int DEFAULT NULL,
  `uso` varchar(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci DEFAULT NULL,
  `tipo` varchar(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci DEFAULT NULL,
  `precio` double(10,2) NOT NULL,
  `disponible` varchar(10) DEFAULT NULL,
  `estadoIn` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id_Inmuebles`, `propietarioId`, `latitud`, `longitud`, `ubicacion`, `direccion`, `ambientes`, `uso`, `tipo`, `precio`, `disponible`, `estadoIn`) VALUES
(7, 5, -3.3137032045789184e16, -6.435607987355648e15, '-33.137032119366346, -64.35608052511462', 'Peron 268', 4, 'Comercial', 'Departamento', 152300.00, 'No', 1),
(8, 6, -3.369104148791296e15, 1.5102258211454976e16, '(-33.85500276210139, 151.02258036323315)', 'Europa 236', 2, 'Comercial', 'Deposito', 270000.00, 'Si', 1),
(14, 5, -3.369104148791296e15, -654519662804992, '(-33.69104091542065, -65.4519682740194)', 'Chile 268', 5, 'Residencial', 'Departamento', 250000.00, 'Si', 1),
(18, 6, -3.472917266432e15, 1.5076240373317632e16, '-34.72917189931966, 150.76240804480017', 'Europa', 4, 'Comercial', 'Casa', 230000.00, 'Si', 1),
(19, 5, -3.443218372886528e15, 1.5067451796488192e16, '-34.43218354680672, 150.67451741980017', 'Europa 446', 2, 'Residencial', 'Departamento', 152300.00, 'Si', 1),
(20, 5, -3.2491133388980224e16, 1.4560693973942272e16, '-32.491134347759846, 145.60693559953992', 'Irymple Mailbox', 1, 'Comercial', 'Local', 152300.00, 'No', 1),
(21, 8, -3.393311823869991e15, 1.5090473210971752e16, '-33.93311823869991, 150.90473210971751', 'Reilly', 2, 'Residencial', 'Casa', 350789.00, 'No', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id_Inquilino` int NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) DEFAULT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `estado` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id_Inquilino`, `nombre`, `apellido`, `dni`, `telefono`, `email`, `estado`) VALUES
(4, 'Gabriel', 'Lucero', '11203654', '2664856321', 'gabriel@mail.com', '1'),
(5, 'Eliana Elina', 'Maldocena', '25865741', '2657425698', 'eli@gmail.com', '1'),
(6, 'Pablo', 'Oviedo', '23569874', '2657896574', 'pablo@gmail.com', '1');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `PagoId` int NOT NULL,
  `contratoId` int NOT NULL,
  `NumeroPago` int DEFAULT NULL,
  `Concepto` varchar(100) DEFAULT NULL,
  `FechaPago` date NOT NULL,
  `Importe` double NOT NULL,
  `EstadoPago` varchar(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`PagoId`, `contratoId`, `NumeroPago`, `Concepto`, `FechaPago`, `Importe`, `EstadoPago`) VALUES
(69, 28, 1, 'Abril', '2024-04-07', 153478, 'Abono'),
(100, 49, 1, 'abril', '2024-05-17', 120500, 'Abono'),
(101, 49, 2, 'Mayo', '2024-05-17', 120500, 'Abono'),
(102, 49, 3, 'Junio', '2024-06-17', 120500, 'Abono'),
(103, 49, 4, 'Julio', '2024-07-17', 120500, 'Abono'),
(107, 49, 5, 'Agosto', '2024-08-17', 120500, 'Abono'),
(109, 52, 1, 'agosto', '2024-08-18', 145500, 'Abono'),
(110, 52, 2, 'Septiembre', '2024-09-17', 145500, 'Abono'),
(113, 52, 3, 'Octubre', '2024-10-17', 145500, 'Abono'),
(114, 53, 1, 'enero', '2025-01-01', 145500, 'Abono'),
(115, 54, 1, 'abril', '2024-04-27', 350268.74, 'Abono'),
(123, 28, 2, 'feb.', '2023-02-16', 20, 'Abono'),
(124, 55, 1, 'abril', '2024-04-18', 350789, 'Abono'),
(125, 55, 2, 'Mayo', '2024-05-18', 350789, 'Abono'),
(126, 56, 1, 'abril', '2024-04-19', 152300, 'Abono'),
(145, 60, 1, 'abril', '2024-04-18', 145500, 'Abono'),
(146, 60, 2, 'may.', '2024-05-12', 145500, 'Multa por terminación anticipada'),
(147, 56, 2, 'Mayo', '2024-05-23', 152300, 'Abono'),
(148, 28, 3, 'Marzo', '2024-03-23', 10, 'Abono');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id_Propietario` int NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) DEFAULT NULL,
  `dni` varchar(50) NOT NULL,
  `email` varchar(45) NOT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `estadoP` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_Propietario`, `nombre`, `apellido`, `dni`, `email`, `telefono`, `estadoP`) VALUES
(5, 'Eduardo', 'Maldonado', '37505981', 'maldonado19994@gmail.com', '2657244097', 1),
(6, 'Fabian', 'Godoy', '26745896', 'fabian@gmail.com', '2657345689', 1),
(7, 'Nicolas', 'Oviedo', '25869741', 'nico@gmail.com', '2657244099', 1),
(8, 'Maria', 'Mottino', '13765482', 'mottino@gmail.com', '2657425698', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `UsuarioId` int NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `apellido` varchar(255) NOT NULL,
  `password` varchar(2000) NOT NULL,
  `correo` varchar(255) NOT NULL,
  `rol` int NOT NULL,
  `avatar` varchar(2000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`UsuarioId`, `nombre`, `apellido`, `password`, `correo`, `rol`, `avatar`) VALUES
(1, 'Javier', 'Oviedo', 'j5KTebbJtWr+2M6DTbL/rZ23ptDRX9NpsrJwttDzdjQ=', 'javi@gmail.com', 1, '/Uploads\\avatar_519197b1-e9d0-45e4-aeb9-93719d8f7f46.png'),
(2, 'Jazmin', 'Nina', 'j5KTebbJtWr+2M6DTbL/rZ23ptDRX9NpsrJwttDzdjQ=', 'nina@gmail.com', 2, '/Uploads\\avatar_7c85d84f-0ee9-471d-b44e-027ba13f13a5.png'),
(3, 'Leonel', 'Messi', 'j5KTebbJtWr+2M6DTbL/rZ23ptDRX9NpsrJwttDzdjQ=', 'leo2022@gmail.com', 1, '/Uploads\\avatar_d03450b2-13d1-4b2b-82c4-db2975774e8c.png');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD PRIMARY KEY (`IdAuditor`);

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id_Contrato`),
  ADD KEY `inmuebleId` (`inmuebleId`),
  ADD KEY `inquilinoId` (`inquilinoId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id_Inmuebles`),
  ADD KEY `propietarioId` (`propietarioId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id_Inquilino`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`PagoId`),
  ADD KEY `contratoId` (`contratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_Propietario`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`UsuarioId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  MODIFY `IdAuditor` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id_Contrato` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=61;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id_Inmuebles` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id_Inquilino` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `PagoId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=149;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_Propietario` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `UsuarioId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`inmuebleId`) REFERENCES `inmuebles` (`id_Inmuebles`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`inquilinoId`) REFERENCES `inquilinos` (`id_Inquilino`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`propietarioId`) REFERENCES `propietarios` (`id_Propietario`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`contratoId`) REFERENCES `contratos` (`id_Contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
