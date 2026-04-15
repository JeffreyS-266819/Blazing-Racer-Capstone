const express = require("express");
const mysql = require("mysql2");
const cors = require("cors");

const app = express();

// middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// database connection
const db = mysql.createConnection({
    host: "localhost",
    user: "SCC",
    password: "SCC",
    database: "blazing_racer"
});

db.connect(err => {
    if (err) {
        console.log("Database connection failed");
        console.log(err);
        return;
    }
    console.log("Connected to MySQL");
});

// ----------------- TEST -----------------
app.get("/test", (req, res) => {
    res.send("Backend is working");
});

// ----------------- LOGIN -----------------
app.post("/login", (req, res) => {
    const username = req.body.username?.trim();
    const password = req.body.password?.trim();

    if (!username || !password)
        return res.send("Enter username and password");

    const sql = "SELECT * FROM users WHERE username = ?";

    db.query(sql, [username], (err, result) => {
        if (err) return res.send("Database error");

        if (result.length === 0)
            return res.send("User does not exist");

        const user = result[0];

        if (user.password === password) {
            // IMPORTANT: send user_id to Unity
            return res.send("SUCCESS|" + user.user_id);
        } else {
            return res.send("Incorrect password");
        }
    });
});

// ----------------- CREATE ACCOUNT -----------------
app.post("/create", (req, res) => {
    const username = req.body.username?.trim();
    const password = req.body.password?.trim();

    if (!username || !password)
        return res.send("Enter username and password");

    const checkUser = "SELECT * FROM users WHERE username = ?";

    db.query(checkUser, [username], (err, result) => {
        if (err) return res.send("Database error");

        if (result.length > 0)
            return res.send("Username already exists");

        const insert = "INSERT INTO users (username, password) VALUES (?, ?)";

        db.query(insert, [username, password], (err) => {
            if (err) return res.send("Database error");

            res.send("Account created");
        });
    });
});

// ----------------- SAVE RACE TIME -----------------
app.post("/saveRace", (req, res) => {
    const user_id = req.body.user_id;
    const lap_time = req.body.lap_time;
    const date_played = req.body.date_played;

    if (!user_id || !lap_time)
        return res.send("Missing data");

    const sql = `
        INSERT INTO RaceAttempts (user_id, lap_time, date_played)
        VALUES (?, ?, ?)
    `;

    db.query(sql, [user_id, lap_time, date_played], (err) => {
        if (err) {
            console.log(err);
            return res.send("Database error");
        }

        res.send("Saved");
    });
});

// ----------------- LEADERBOARD -----------------
app.get("/leaderboard", (req, res) => {
    const sql = `
        SELECT u.username, r.lap_time
        FROM RaceAttempts r
        JOIN users u ON r.user_id = u.user_id
        ORDER BY r.lap_time ASC
        LIMIT 5
    `;

    db.query(sql, (err, results) => {
        if (err) {
            console.log(err);
            return res.status(500).json({ error: "Database error" });
        }

        res.json(results);
    });
});

// ----------------- START SERVER -----------------
app.listen(3000, () => {
    console.log("Server running on port 3000");
});