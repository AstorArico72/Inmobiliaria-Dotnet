-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 22-06-2024 a las 16:44:59
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

--
-- Volcado de datos para la tabla `Contratos`
--

INSERT INTO `Contratos` (`ID`, `Locatario`, `Propiedad`, `FechaLímite`, `FechaContrato`, `Vigente`, `Monto`) VALUES
(3, 1, 1, '0001-01-01 00:00:00', '2023-04-18 15:19:38', 0, 0),
(6, 1, 14, '2024-05-05 19:05:27', '2023-04-18 15:19:38', 0, 0),
(7, 1, 2, '2024-05-05 19:06:38', '2023-04-18 17:25:09', 0, 0),
(10, 1, 15, '2024-05-05 19:07:01', '0001-01-01 00:00:00', 0, 0),
(14, 1, 16, '2024-05-05 19:07:27', '2024-06-25 00:00:00', 0, 0),
(22, 1, 18, '2024-05-05 19:08:52', '2024-05-10 00:00:00', 0, 0),
(23, 1, 1, '2024-05-05 19:12:57', '2024-01-01 00:00:00', 0, 0),
(24, 1, 14, '2024-05-05 19:15:35', '2023-06-01 00:00:00', 0, 0),
(25, 1, 16, '2024-05-05 19:18:56', '2024-01-01 00:00:00', 0, 0),
(26, 1, 16, '2024-05-05 19:19:49', '2023-01-01 00:00:00', 0, 0),
(27, 1, 16, '2024-05-05 19:22:38', '2024-01-01 00:00:00', 0, 0),
(28, 6, 1, '2024-05-23 12:42:09', '1999-01-01 00:00:00', 0, 0),
(29, 1, 3, '2024-05-05 19:25:29', '2024-02-01 00:00:00', 0, 0),
(30, 1, 18, '2024-05-05 19:26:08', '2025-01-01 00:00:00', 0, 0),
(31, 1, 14, '2024-05-05 19:30:44', '2024-07-01 00:00:00', 0, 0),
(32, 1, 15, '2024-05-05 19:33:30', '2024-07-01 00:00:00', 0, 0),
(33, 1, 14, '2024-05-05 19:39:33', '2024-07-01 00:00:00', 0, 0),
(34, 1, 17, '2024-05-05 19:40:00', '2024-04-01 00:00:00', 0, 0),
(35, 7, 18, '2024-06-10 11:44:52', '2024-06-01 00:00:00', 0, 0),
(36, 1, 17, '2024-05-05 19:43:17', '2024-06-01 00:00:00', 0, 0),
(37, 1, 14, '2024-05-05 19:44:46', '2024-04-01 00:00:00', 0, 0),
(38, 1, 1, '2024-05-05 19:52:35', '2024-01-01 00:00:00', 0, 0),
(39, 1, 3, '2024-05-05 19:52:54', '2024-04-01 00:00:00', 0, 0),
(40, 10, 16, '2025-01-01 00:00:00', '2024-09-01 00:00:00', 1, 0),
(41, 7, 14, '2024-05-05 19:58:08', '2024-01-01 00:00:00', 0, 0),
(42, 6, 20, '2024-09-10 00:00:00', '2024-05-10 00:00:00', 1, 0),
(43, 6, 14, '2024-11-11 00:00:00', '2024-05-11 00:00:00', 1, 0),
(44, 1, 3, '2024-09-10 00:00:00', '2024-06-10 00:00:00', 1, 165000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inmuebles`
--

CREATE TABLE `Inmuebles` (
  `ID` int(11) NOT NULL,
  `Dirección` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `Superficie` smallint(6) NOT NULL,
  `Precio` int(11) DEFAULT NULL,
  `Propietario` int(11) NOT NULL,
  `Tipo` varchar(32) COLLATE utf8_bin DEFAULT NULL,
  `Uso` varchar(32) COLLATE utf8_bin DEFAULT NULL,
  `Ambientes` tinyint(4) NOT NULL DEFAULT 0,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1,
  `CoordenadasX` float NOT NULL DEFAULT 0,
  `CoordenadasY` float NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Volcado de datos para la tabla `Inmuebles`
--

INSERT INTO `Inmuebles` (`ID`, `Dirección`, `Superficie`, `Precio`, `Propietario`, `Tipo`, `Uso`, `Ambientes`, `Disponible`, `CoordenadasX`, `CoordenadasY`) VALUES
(1, '1001 Calle 123', 200, 485000, 4, 'Casa', 'Personal', 10, 1, -64.0989, -32.7735),
(2, '1301 Calle 123', 450, 165000, 4, 'Casa', 'Personal', 12, 1, -64.1083, -32.7699),
(3, '3201 Calle 123', 250, 112000, 5, 'Galpón', 'Comercial', 1, 1, -64.1104, -32.768),
(14, '835 Park Drive Dept. 1211', 103, 119000, 6, 'Departamento', 'Personal', 4, 1, -64.1195, -32.6839),
(15, '10333 Park Drive', 58, 750000, 7, 'Local', 'Comercial', 3, 1, -63.9901, -32.6584),
(16, '3972 Cerezo Ave. Dept. 1209', 75, 250000, 1, 'Departamento', 'Personal', 3, 1, -64.1873, -32.7239),
(17, '10091 Calle 94', 249, 244000, 5, 'Galpón', 'Personal', 2, 1, -64.1752, -32.6911),
(18, 'Km 1039 Ruta Nacional 7', 4000, 1200000, 5, 'Terreno', 'Personal', 0, 1, -63.7501, -32.5776),
(19, '284 Calle 10', 60, 350000, 4, 'Local', 'Comercial', 1, 0, -64.1303, -32.7894),
(20, '3028 Calle 15', 84, 600000, 5, 'Local', 'Comercial', 3, 1, -64.1279, -32.7642);

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

--
-- Volcado de datos para la tabla `Inquilinos`
--

INSERT INTO `Inquilinos` (`ID`, `Nombre`, `DNI`, `Contacto`) VALUES
(1, 'Vivo aquí', '48D80BE1', 'dummy@example.net'),
(4, 'Jane Doe', '4A29FB9C', 'jdoe@example.com'),
(5, 'Richard Roe', '40F713D9', 'rroe@example.org'),
(6, 'Sin identificar', 'No', 'Sin especificar'),
(7, 'Rebecca Goldstein', '3E01D64A', 'goldstein@example.net'),
(8, 'Marvin von Gelb', '3FF087D7', 'gelb@example.net'),
(9, 'Mariano Luzza', '51BA87EC', 'luzza@example.net'),
(10, 'Ludwig van Patten', '5408CB7E', 'ludwig@vanpattenlaw.com - 555-8001 - 1157 Spring Ave.'),
(11, 'Yuuki Yamazaki', '54E9077D', 'yamazaki@example.net'),
(12, 'Fang Liangxing', '54EB593A', 'fang@example.net'),
(13, 'Astor F. Aricó', '54FF220E', 'astorarico72@gmail.com');

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

--
-- Volcado de datos para la tabla `Pagos`
--

INSERT INTO `Pagos` (`ID`, `NumeroPago`, `IdContrato`, `Monto`, `FechaPago`, `Pagado`) VALUES
(1, 1, 3, 44200, '2024-04-15 00:00:00', 1),
(3, 2, 3, 48500, '2024-04-15 00:00:00', 1),
(4, 3, 3, 48500, '2024-04-15 17:42:48', 0),
(5, 1, 6, 97000, '2024-04-15 00:00:00', 1),
(7, 1, 14, 145000, '2024-04-15 17:42:48', 0),
(9, 2, 7, 165000, '2024-04-15 00:00:00', 1),
(14, 1, 22, 1200000, '0001-01-01 00:00:00', 1),
(15, 2, 22, 1200000, '2024-06-10 00:00:00', 1),
(16, 3, 22, 1200000, '2024-07-10 00:00:00', 0),
(17, 4, 22, 1200000, '2024-08-10 00:00:00', 0),
(18, 5, 22, 1200000, '2024-09-10 00:00:00', 0),
(19, 6, 22, 1200000, '2024-10-10 00:00:00', 0),
(20, 7, 22, 1200000, '2024-11-10 00:00:00', 0),
(21, 8, 22, 1200000, '2024-12-10 00:00:00', 0),
(22, 9, 22, 1200000, '2025-01-10 00:00:00', 0),
(23, 10, 22, 1200000, '2025-02-10 00:00:00', 0),
(24, 11, 22, 1200000, '2025-03-10 00:00:00', 0),
(25, 12, 22, 1200000, '2025-04-10 00:00:00', 0),
(26, 13, 22, 1200000, '2025-05-10 00:00:00', 0),
(27, 1, 23, 485000, '2024-01-01 00:00:00', 0),
(28, 2, 23, 485000, '2024-02-01 00:00:00', 0),
(29, 3, 23, 485000, '2024-03-01 00:00:00', 0),
(30, 4, 23, 485000, '2024-04-01 00:00:00', 0),
(31, 5, 23, 485000, '2024-05-01 00:00:00', 0),
(32, 6, 23, 485000, '2024-06-01 00:00:00', 0),
(33, 7, 23, 485000, '2024-07-01 00:00:00', 0),
(34, 8, 23, 485000, '2024-08-01 00:00:00', 0),
(35, 9, 23, 485000, '2024-09-01 00:00:00', 0),
(36, 10, 23, 485000, '2024-10-01 00:00:00', 0),
(37, 11, 23, 485000, '2024-11-01 00:00:00', 0),
(38, 12, 23, 485000, '2024-12-01 00:00:00', 0),
(39, 13, 23, 485000, '2025-01-01 00:00:00', 0),
(40, 14, 23, 485000, '2025-02-01 00:00:00', 0),
(41, 15, 23, 485000, '2025-03-01 00:00:00', 0),
(42, 16, 23, 485000, '2025-04-01 00:00:00', 0),
(43, 17, 23, 485000, '2025-05-01 00:00:00', 0),
(44, 18, 23, 485000, '2025-06-01 00:00:00', 0),
(45, 19, 23, 485000, '2025-07-01 00:00:00', 0),
(46, 20, 23, 485000, '2025-08-01 00:00:00', 0),
(47, 21, 23, 485000, '2025-09-01 00:00:00', 0),
(48, 22, 23, 485000, '2025-10-01 00:00:00', 0),
(49, 23, 23, 485000, '2025-11-01 00:00:00', 0),
(50, 24, 23, 485000, '2025-12-01 00:00:00', 0),
(51, 1, 24, 119000, '2023-06-01 00:00:00', 0),
(52, 2, 24, 119000, '2023-07-01 00:00:00', 0),
(53, 3, 24, 119000, '2023-08-01 00:00:00', 0),
(54, 4, 24, 119000, '2023-09-01 00:00:00', 0),
(55, 5, 24, 119000, '2023-10-01 00:00:00', 0),
(56, 6, 24, 119000, '2023-11-01 00:00:00', 0),
(57, 7, 24, 119000, '2023-12-01 00:00:00', 0),
(58, 8, 24, 119000, '2024-01-01 00:00:00', 0),
(59, 9, 24, 119000, '2024-02-01 00:00:00', 0),
(60, 10, 24, 119000, '2024-03-01 00:00:00', 0),
(61, 11, 24, 119000, '2024-04-01 00:00:00', 0),
(62, 12, 24, 119000, '2024-05-01 00:00:00', 0),
(63, 1, 25, 250000, '2024-01-01 00:00:00', 0),
(64, 2, 25, 250000, '2024-02-01 00:00:00', 0),
(65, 3, 25, 250000, '2024-03-01 00:00:00', 0),
(66, 4, 25, 250000, '2024-04-01 00:00:00', 0),
(67, 5, 25, 250000, '2024-05-01 00:00:00', 0),
(68, 6, 25, 250000, '2024-06-01 00:00:00', 0),
(69, 7, 25, 250000, '2024-07-01 00:00:00', 0),
(70, 8, 25, 250000, '2024-08-01 00:00:00', 0),
(71, 9, 25, 250000, '2024-09-01 00:00:00', 0),
(72, 10, 25, 250000, '2024-10-01 00:00:00', 0),
(73, 11, 25, 250000, '2024-11-01 00:00:00', 0),
(74, 12, 25, 250000, '2024-12-01 00:00:00', 0),
(75, 1, 26, 250000, '2023-01-01 00:00:00', 0),
(76, 2, 26, 250000, '2023-02-01 00:00:00', 0),
(77, 3, 26, 250000, '2023-03-01 00:00:00', 0),
(78, 4, 26, 250000, '2023-04-01 00:00:00', 0),
(79, 5, 26, 250000, '2023-05-01 00:00:00', 0),
(80, 6, 26, 250000, '2023-06-01 00:00:00', 0),
(81, 7, 26, 250000, '2023-07-01 00:00:00', 0),
(82, 8, 26, 250000, '2023-08-01 00:00:00', 0),
(83, 9, 26, 250000, '2023-09-01 00:00:00', 0),
(84, 10, 26, 250000, '2023-10-01 00:00:00', 0),
(85, 11, 26, 250000, '2023-11-01 00:00:00', 0),
(86, 12, 26, 250000, '2023-12-01 00:00:00', 0),
(87, 1, 27, 250000, '2024-01-01 00:00:00', 0),
(88, 2, 27, 250000, '2024-02-01 00:00:00', 0),
(89, 3, 27, 250000, '2024-03-01 00:00:00', 0),
(90, 4, 27, 250000, '2024-04-01 00:00:00', 0),
(91, 5, 27, 250000, '2024-05-01 00:00:00', 0),
(92, 6, 27, 250000, '2024-06-01 00:00:00', 0),
(93, 7, 27, 250000, '2024-07-01 00:00:00', 0),
(94, 8, 27, 250000, '2024-08-01 00:00:00', 0),
(95, 9, 27, 250000, '2024-09-01 00:00:00', 0),
(96, 10, 27, 250000, '2024-10-01 00:00:00', 0),
(97, 11, 27, 250000, '2024-11-01 00:00:00', 0),
(98, 1, 28, 485000, '1999-01-01 00:00:00', 1),
(99, 1, 29, 112000, '2024-02-01 00:00:00', 0),
(100, 1, 30, 1200000, '2025-01-01 00:00:00', 0),
(101, 1, 31, 119000, '2024-07-01 00:00:00', 0),
(102, 1, 32, 750000, '2024-07-01 00:00:00', 0),
(103, 1, 33, 119000, '2024-07-01 00:00:00', 0),
(104, 1, 34, 244000, '2024-04-01 00:00:00', 0),
(105, 1, 35, 1200000, '2024-06-01 00:00:00', 1),
(106, 1, 36, 244000, '2024-06-01 00:00:00', 0),
(107, 1, 37, 119000, '2024-04-01 00:00:00', 0),
(108, 2, 37, 119000, '2024-05-01 00:00:00', 0),
(109, 3, 37, 119000, '2024-06-01 00:00:00', 0),
(110, 4, 37, 119000, '2024-07-01 00:00:00', 0),
(111, 5, 37, 119000, '2024-08-01 00:00:00', 0),
(112, 6, 37, 119000, '2024-09-01 00:00:00', 0),
(113, 7, 37, 119000, '2024-10-01 00:00:00', 0),
(114, 8, 37, 119000, '2024-11-01 00:00:00', 0),
(115, 9, 37, 119000, '2024-12-01 00:00:00', 0),
(116, 10, 37, 119000, '2025-01-01 00:00:00', 0),
(117, 11, 37, 119000, '2025-02-01 00:00:00', 0),
(118, 12, 37, 119000, '2025-03-01 00:00:00', 0),
(119, 13, 37, 119000, '2025-04-01 00:00:00', 0),
(120, 1, 38, 485000, '2024-01-01 00:00:00', 0),
(121, 2, 38, 485000, '2024-02-01 00:00:00', 0),
(122, 3, 38, 485000, '2024-03-01 00:00:00', 0),
(123, 1, 39, 112000, '2024-04-01 00:00:00', 0),
(124, 2, 39, 112000, '2024-05-01 00:00:00', 0),
(125, 3, 39, 112000, '2024-06-01 00:00:00', 0),
(126, 4, 39, 112000, '2024-07-01 00:00:00', 0),
(127, 5, 39, 112000, '2024-08-01 00:00:00', 0),
(128, 1, 40, 250000, '2024-09-01 00:00:00', 1),
(129, 2, 40, 250000, '2024-10-01 00:00:00', 2),
(130, 3, 40, 250000, '2024-11-01 00:00:00', 0),
(131, 4, 40, 250000, '2024-12-01 00:00:00', 0),
(132, 1, 41, 119000, '2024-01-01 00:00:00', 0),
(133, 2, 41, 119000, '2024-02-01 00:00:00', 0),
(134, 3, 41, 119000, '2024-03-01 00:00:00', 0),
(135, 4, 41, 119000, '2024-04-01 00:00:00', 0),
(136, 5, 41, 119000, '2024-05-01 00:00:00', 0),
(137, 1, 42, 600000, '2024-05-10 00:00:00', 1),
(138, 2, 42, 600000, '2024-06-10 00:00:00', 1),
(139, 3, 42, 600000, '2024-07-10 00:00:00', 0),
(140, 4, 42, 600000, '2024-08-10 00:00:00', 0),
(141, 1, 43, 119000, '2024-05-11 00:00:00', 0),
(142, 2, 43, 119000, '2024-06-11 00:00:00', 0),
(143, 3, 43, 119000, '2024-07-11 00:00:00', 0),
(144, 4, 43, 119000, '2024-08-11 00:00:00', 0),
(145, 5, 43, 119000, '2024-09-11 00:00:00', 0),
(146, 6, 43, 119000, '2024-10-11 00:00:00', 0),
(147, 1, 44, 165000, '2024-06-10 00:00:00', 0),
(148, 2, 44, 165000, '2024-07-10 00:00:00', 0),
(149, 3, 44, 165000, '2024-08-10 00:00:00', 0);

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

--
-- Volcado de datos para la tabla `Propietarios`
--

INSERT INTO `Propietarios` (`ID`, `Nombre`, `Contacto`, `DNI`) VALUES
(1, 'Nombre editado', '\"Sin especificar\"', ''),
(4, 'Juan Pérez', '  Teléfono: 839-992-5507. Correo: j-perez@example.net ', '3F0912E4'),
(5, 'John Doe', '839-387-2201', ''),
(6, 'Joao da Silva', '  jdasilva@dotnet-realtors.com ', '3E9A8DE1'),
(7, 'Joaquim Ribera', ' jribera@dotnet-realtors.com', '');

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
-- Volcado de datos para la tabla `Usuarios`
--

INSERT INTO `Usuarios` (`ID`, `Nombre`, `Clave`, `Rol`, `UrlImagen`) VALUES
(12, 'Admin-3', 'ykxZ4qarZsJX8Y9kbVV+YlD3JQQkwGnzBpKZguYzEjU=', 'Admin', '/medios/Foto_03a030dd-a014-4dfc-9067-4fe476317fbd.png'),
(15, 'Admin-4', '2YtoOzjBzRID0/h24nW8UQ0YP+mcHjRhK+leNyh1Eak=', 'Admin', '/medios/Foto_0ce699f4-38ad-4a99-8d5c-89d329054116.png'),
(17, 'Empleado', 'ERQ6vgodXI/l151Z8cJCeevlvisZ+gv+Enuf2UZ330U=', 'Empleado', '/medios/Nulo.png');

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
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=45;

--
-- AUTO_INCREMENT de la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `Inquilinos`
--
ALTER TABLE `Inquilinos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `Pagos`
--
ALTER TABLE `Pagos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=150;

--
-- AUTO_INCREMENT de la tabla `Propietarios`
--
ALTER TABLE `Propietarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

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
