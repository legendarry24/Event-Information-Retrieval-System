const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/api/events"
    ],
    target: "https://localhost:7006",
    secure: false
  }
];

module.exports = PROXY_CONFIG;
