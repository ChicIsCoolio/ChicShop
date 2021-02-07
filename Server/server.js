const fs = require("fs");
const express = require("express");
const app = express();

app.use(express.static("/home/runner/ChicShop/Output"));

function getShops(then) {
    fs.readdir('/home/runner/ChicShop/Output', (err, files) => {
        var shops = [];
        
        files.forEach(file => {
            var dateString = file.replace(".png", "").replace(".jpg", "").replace("_small", "");
            var split = dateString.split("-");
            var date = new Date(split[2], split[1], split[0]);

            shops.push({ date: date.toISOString(), fileName: file, file: "https://ChicShop.chiciscoolio.repl.co/" + file });
        })

        then(shops);
    });
}

function getLatest(then) {
    getShops(shops => {;
        var date = new Date(Math.max.apply(null, shops.map(function(e) {
            return new Date(e.date);
        })));

        var fileName = shops.find(e => e.date == date.toISOString()).fileName;

        var shop = { date: date, fileName: fileName, file: "https://ChicShop.chiciscoolio.repl.co/" + fileName };

        then(shop);
    });
}

app.get('/', (req, res) => {
    getLatest(shop => {
        res.redirect(shop.file);
    });
});

app.get('/latest', (req, res) => {
    getLatest(shop => {
        res.send(shop);
    });
});

app.get('/shops', (req, res) => {
    getShops(shops => {
        res.send(shops);
    });
});

app.listen(8080, () => {
    console.log("Server listening on port: 8080");
});