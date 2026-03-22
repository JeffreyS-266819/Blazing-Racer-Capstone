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
    user: "root",
    password: "asd060305",
    database: "blazing_racer"
});

// connect to database
db.connect(err => {
    if (err) {
        console.log("Database connection failed");
        console.log(err);
        return;
    }
    console.log("Connected to MySQL");
});

// test route
app.get("/test", (req, res) => {
    res.send("Backend is working");
});

// login / register route
app.post("/login", (req, res) => {

    const { username, password } = req.body;

    console.log("Login attempt:", username);

    if (!username || !password) {
        return res.send("Enter username and password");
    }

    const sql = "SELECT * FROM users WHERE username = ?";

    db.query(sql, [username], (err, result) => {

        if (err) {
            console.log(err);
            return res.send("Database error");
        }

        // user exists
        if (result.length > 0) {

            if (result[0].password === password) {
                res.send("Login successful");
            } else {
                res.send("Incorrect password");
            }

        }
        // user does not exist → create account
        else {

            const insert = "INSERT INTO users (username, password) VALUES (?, ?)";

            db.query(insert, [username, password], (err) => {

                if (err) {
                    console.log(err);
                    return res.send("Username already exists");
                }

                res.send("Account created");
            });

        }

    });

});

// start server
app.listen(3000, () => {
    console.log("Server running on port 3000");
});