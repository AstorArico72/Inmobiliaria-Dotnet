-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 16-06-2024 a las 19:11:06
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
  `Locatario` int(11) NOT NULL,
  `Propiedad` int(11) NOT NULL,
  `FechaLímite` datetime NOT NULL,
  `FechaContrato` datetime NOT NULL DEFAULT current_timestamp(),
  `Vigente` tinyint(1) NOT NULL DEFAULT 1,
  `Monto` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inmuebles`
--

CREATE TABLE `Inmuebles` (
  `ID` int(11) NOT NULL,
  `Dirección` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `Superficie` smallint(6) NOT NULL,
  `Precio` int(11) DEFAULT NULL,
  `Propietario` int(11) DEFAULT NULL,
  `Tipo` varchar(32) COLLATE utf8_bin DEFAULT NULL,
  `Uso` varchar(32) COLLATE utf8_bin DEFAULT NULL,
  `Ambientes` tinyint(4) NOT NULL DEFAULT 0,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1,
  `CoordenadasX` float NOT NULL DEFAULT 0,
  `CoordenadasY` float NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inquilinos`
--

CREATE TABLE `Inquilinos` (
  `ID` int(11) NOT NULL,
  `Nombre` varchar(128) COLLATE utf8_bin NOT NULL,
  `DNI` varchar(10) COLLATE utf8_bin NOT NULL,
  `Contacto` varchar(1000) COLLATE utf8_bin DEFAULT 'Sin especificar'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Pagos`
--

CREATE TABLE `Pagos` (
  `ID` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `Monto` int(11) NOT NULL,
  `FechaPago` datetime NOT NULL DEFAULT current_timestamp(),
  `Pagado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Propietarios`
--

CREATE TABLE `Propietarios` (
  `ID` int(11) NOT NULL,
  `Nombre` varchar(128) COLLATE utf8_bin NOT NULL,
  `Contacto` varchar(1000) COLLATE utf8_bin DEFAULT '"Sin especificar"',
  `DNI` varchar(10) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Usuarios`
--

CREATE TABLE `Usuarios` (
  `ID` int(11) NOT NULL,
  `Nombre` varchar(255) COLLATE utf8_bin NOT NULL,
  `Clave` varchar(255) COLLATE utf8_bin NOT NULL,
  `Rol` varchar(32) COLLATE utf8_bin NOT NULL,
  `UrlImagen` varchar(255) COLLATE utf8_bin DEFAULT '/medios/Nulo.png'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `Contratos`
--
ALTER TABLE `Contratos`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `Locatario` (`Locatario`),
  ADD KEY `Propiedad` (`Propiedad`);

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
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Inquilinos`
--
ALTER TABLE `Inquilinos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Pagos`
--
ALTER TABLE `Pagos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Propietarios`
--
ALTER TABLE `Propietarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `Contratos`
--
ALTER TABLE `Contratos`
  ADD CONSTRAINT `Contratos_ibfk_2` FOREIGN KEY (`Locatario`) REFERENCES `Inquilinos` (`ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `Contratos_ibfk_3` FOREIGN KEY (`Propiedad`) REFERENCES `Inmuebles` (`ID`) ON DELETE CASCADE;

--
-- Filtros para la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  ADD CONSTRAINT `Inmuebles_ibfk_1` FOREIGN KEY (`Propietario`) REFERENCES `Propietarios` (`ID`);

--
-- Filtros para la tabla `Pagos`
--
ALTER TABLE `Pagos`
  ADD CONSTRAINT `Pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `Contratos` (`ID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
