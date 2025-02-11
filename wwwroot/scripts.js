document.getElementById("searchButton").addEventListener("click", searchGames);
document.getElementById("browseAllButton").addEventListener("click", browseAllGames);
document.getElementById("sortSelect").addEventListener("change", function () {
    const sortBy = this.value;
    sortGames(sortBy);
});

const filterContainer = document.querySelector(".filter-container");
filterContainer.style.display = "none"; // Hide as default

async function searchGames() {
    const query = document.getElementById("searchInput").value.trim().toLowerCase();
    if (!query) return;

    // Retrieve list of games
    const response = await fetch("http://localhost:5099/api/games/all");
    if (!response.ok) {
        console.error("Error fetching games:", response.statusText);
        return;
    }

    const games = await response.json();

    // Filter based on name, category or platform
    const filteredGames = games.filter(game =>
        game.title.toLowerCase().includes(query) ||
        game.genre.toLowerCase().includes(query) ||
        game.platform.toLowerCase().includes(query)
    );

    displayGames(filteredGames);
}

async function browseAllGames() {
    const response = await fetch("http://localhost:5099/api/games/all");
    if (!response.ok) {
        console.error("Error fetching games:", response.statusText);
        return;
    }
    
    const games = await response.json();
    displayGames(games);
    sortGames("name"); // Filter by name as default

    // Show filter when games are displayed
    filterContainer.style.display = "block";
}

function displayGames(games) {
    const gameList = document.getElementById("gameList");
    gameList.innerHTML = "";

    if (games.length === 0) {
        gameList.innerHTML = "<p>No games found.</p>";
        filterContainer.style.display = "none"; // Hide filtering when no games to display
        return;
    }

    games.forEach(game => {
        const gameItem = document.createElement("div");
        gameItem.classList.add("game-item");
        gameItem.dataset.name = game.title;
        gameItem.dataset.category = game.genre;
        gameItem.dataset.platform = game.platform;
        gameItem.dataset.publisher = game.publisher;
        gameItem.innerHTML = `<span>${game.title} - ${game.genre} - ${game.platform} - ${game.publisher}</span>`;
        gameList.appendChild(gameItem);
    });
}

function sortGames(sortBy) {
    const gameList = document.getElementById("gameList");
    let games = Array.from(gameList.children);

    games.sort((a, b) => {
        let textA, textB;
        switch (sortBy) {
            case "name":
                textA = a.dataset.name.toLowerCase();
                textB = b.dataset.name.toLowerCase();
                break;
            case "category":
                textA = a.dataset.category.toLowerCase();
                textB = b.dataset.category.toLowerCase();
                break;
            case "platform":
                textA = a.dataset.platform.toLowerCase();
                textB = b.dataset.platform.toLowerCase();
                break;
            case "publisher":
                textA = a.dataset.publisher.toLowerCase();
                textB = b.dataset.publisher.toLowerCase();
                break;
        }
        return textA.localeCompare(textB);
    });

    gameList.innerHTML = "";
    games.forEach(game => gameList.appendChild(game));
}
