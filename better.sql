-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 11, 2019 at 12:01 AM
-- Server version: 10.1.37-MariaDB
-- PHP Version: 7.3.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `better`
--

-- --------------------------------------------------------

--
-- Table structure for table `que`
--

CREATE TABLE `que` (
  `id` int(254) NOT NULL,
  `player1_uid` text NOT NULL,
  `player2_uid` text NOT NULL,
  `player1_score` int(254) NOT NULL,
  `player2_score` int(254) NOT NULL,
  `bet` int(254) NOT NULL,
  `game` text NOT NULL,
  `player1_conID` int(254) NOT NULL,
  `player2_conID` int(254) NOT NULL,
  `player1_gameOver` tinyint(1) NOT NULL,
  `player2_gameOver` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `que`
--

INSERT INTO `que` (`id`, `player1_uid`, `player2_uid`, `player1_score`, `player2_score`, `bet`, `game`, `player1_conID`, `player2_conID`, `player1_gameOver`, `player2_gameOver`) VALUES
(7, '5b9a82a2d8430f1dddfe4085ba755af6b6873bbb', '5b9a82a2d8430f1dddfe4085ba755af6b6873bbb', 0, 0, 2, 'Snake', 1, 2, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(254) NOT NULL,
  `uid` text NOT NULL,
  `username` text NOT NULL,
  `password` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `uid`, `username`, `password`) VALUES
(2, '5b9a82a2d8430f1dddfe4085ba755af6b6873bbb', 'new_user', NULL);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `que`
--
ALTER TABLE `que`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `que`
--
ALTER TABLE `que`
  MODIFY `id` int(254) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(254) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
