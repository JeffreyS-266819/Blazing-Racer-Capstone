CREATE blazing_racer;
USE blazing_racer;

DROP TABLE IF EXISTS RaceAttempts;
DROP TABLE IF EXISTS users;

CREATE TABLE users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE RaceAttempts (
    attempt_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    lap_time FLOAT NOT NULL,
    date_played DATE,
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);