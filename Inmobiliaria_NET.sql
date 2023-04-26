-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 26-04-2023 a las 22:24:28
-- Versión del servidor: 10.4.21-MariaDB
-- Versión de PHP: 8.0.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `Inmobiliaria_NET`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Contratos`
--

CREATE TABLE `Contratos` (
  `ID` int(11) NOT NULL,
  `Locador` int(11) NOT NULL,
  `Locatario` int(11) NOT NULL,
  `Propiedad` int(11) NOT NULL,
  `FechaLímite` datetime NOT NULL,
  `FechaContrato` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Contratos`
--

INSERT INTO `Contratos` (`ID`, `Locador`, `Locatario`, `Propiedad`, `FechaLímite`, `FechaContrato`) VALUES
(3, 1, 1, 1, '2025-06-01 00:00:00', '2023-04-18 15:19:38'),
(6, 6, 4, 14, '2023-09-15 00:00:00', '2023-04-18 15:19:38'),
(7, 4, 5, 2, '2024-01-01 00:00:00', '2023-04-18 17:25:09'),
(8, 6, 6, 14, '2025-11-10 00:00:00', '2023-04-25 17:06:20');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inmuebles`
--

CREATE TABLE `Inmuebles` (
  `ID` int(11) NOT NULL,
  `Dirección` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `Superficie` smallint(6) NOT NULL,
  `Precio` int(11) DEFAULT NULL,
  `Propietario` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Inmuebles`
--

INSERT INTO `Inmuebles` (`ID`, `Dirección`, `Superficie`, `Precio`, `Propietario`) VALUES
(1, '1001 Calle 123', 200, 48500, 4),
(2, '1301 Calle 123', 450, 165000, 4),
(3, '3201 Calle 123', 250, 90000, NULL),
(14, '835 Park Drive Dept. 1211', 90, 105000, 6);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inquilinos`
--

CREATE TABLE `Inquilinos` (
  `ID` int(11) NOT NULL,
  `Nombre` varchar(128) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Inquilinos`
--

INSERT INTO `Inquilinos` (`ID`, `Nombre`) VALUES
(1, 'Vivo aquí'),
(4, 'Jane Doe'),
(5, 'Richard Roe'),
(6, 'Sin identificar');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Pagos`
--

CREATE TABLE `Pagos` (
  `ID` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `Monto` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Pagos`
--

INSERT INTO `Pagos` (`ID`, `NumeroPago`, `IdContrato`, `Monto`) VALUES
(1, 1, 3, 44200),
(3, 2, 3, 48500),
(4, 3, 3, 48500),
(5, 1, 6, 97000),
(6, 1, 7, 125000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Propietarios`
--

CREATE TABLE `Propietarios` (
  `ID` int(11) NOT NULL,
  `Nombre` varchar(128) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Propietarios`
--

INSERT INTO `Propietarios` (`ID`, `Nombre`) VALUES
(1, 'Nombre editado'),
(4, 'Juan Pérez'),
(5, 'John Doe'),
(6, 'Joao da Silva'),
(7, 'Joaquim Ribera');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Usuarios`
--

CREATE TABLE `Usuarios` (
  `ID` int(11) NOT NULL,
  `Nombre` varchar(255) COLLATE utf8_bin NOT NULL,
  `Clave` varchar(255) COLLATE utf8_bin NOT NULL,
  `Rol` varchar(32) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Usuarios`
--

INSERT INTO `Usuarios` (`ID`, `Nombre`, `Clave`, `Rol`) VALUES
(1, 'Admin', 'BmROJ30GtSj1j85xarEzroDKlwWugbRpdsfIEADmqUU=', 'Admin'),
(3, 'Esclavo-2', 'Pwuh2BYri8iuzpNtRFfnanfICvgQwjkOps72y6w9/yU=', 'Usuario');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `Contratos`
--
ALTER TABLE `Contratos`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `Locatario` (`Locatario`),
  ADD KEY `Propiedad` (`Propiedad`),
  ADD KEY `Locador` (`Locador`);

--
-- Indices de la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `Propietario` (`Propietario`);

--
-- Indices de la tabla `Inquilinos`
--
ALTER TABLE `Inquilinos`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `Pagos`
--
ALTER TABLE `Pagos`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `IdContrato` (`IdContrato`);

--
-- Indices de la tabla `Propietarios`
--
ALTER TABLE `Propietarios`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `Contratos`
--
ALTER TABLE `Contratos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `Inquilinos`
--
ALTER TABLE `Inquilinos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `Pagos`
--
ALTER TABLE `Pagos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `Propietarios`
--
ALTER TABLE `Propietarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `Contratos`
--
ALTER TABLE `Contratos`
  ADD CONSTRAINT `Contratos_ibfk_1` FOREIGN KEY (`Locador`) REFERENCES `Propietarios` (`ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `Contratos_ibfk_2` FOREIGN KEY (`Locatario`) REFERENCES `Inquilinos` (`ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `Contratos_ibfk_3` FOREIGN KEY (`Propiedad`) REFERENCES `Inmuebles` (`ID`) ON DELETE CASCADE;

--
-- Filtros para la tabla `Pagos`
--
ALTER TABLE `Pagos`
  ADD CONSTRAINT `Pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `Contratos` (`ID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
