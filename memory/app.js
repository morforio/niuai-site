//const { validateHeaderName } = require("node:http");

document.addEventListener('DOMContentLoaded', () => {
    //Carregamento dos Cards
    const cardArray = [
        {
            name: "ganhou",
            img: "images/ganhou.png"

        },
        {
            name: "ganhou",
            img: "images/ganhou.png"

        },
        {
            name: "direita",
            img: "images/direita.png"
        },
        {
            name: "direita",
            img: "images/direita.png"
        },
        {
            name: "tras",
            img: "images/tras.png"
        },
        {
            name: "tras",
            img: "images/tras.png"
        },
        {
            name: "correndo",
            img: "images/correndo.png"
        },
        {
            name: "correndo",
            img: "images/correndo.png"
        },
        {
            name: "pulo",
            img: "images/pulo.png"
        },
        {
            name: "pulo",
            img: "images/pulo.png"
        },
        {
            name: "esquerda",
            img: "images/esquerda.png"
        },
        {
            name: "esquerda",
            img: "images/esquerda.png"
        },
    ];

    cardArray.sort(() => 0.5 - Math.random());


    const grid = document.querySelector('.grid');
    var resultDisplay = document.querySelector('#result');
    var contagi = document.getElementById('contagi');
    var cardsChosen = [];
    var cardsChosenId = [];
    var pares = []
    var contador = 0;

    //Criando a tela do jogo
    function createBoard() {
        for (let i = 0; i < cardArray.length; i++) {
            const card = document.createElement('img');
            card.setAttribute('src', 'images/card.png');
            card.setAttribute('data-id', i);
            card.addEventListener('click', flipCard);
            grid.appendChild(card)
        }
    }

    //Conferindo pares
    function checkforMatch() {
        var cards = document.querySelectorAll('img');
        var optionOneId = cardsChosenId[0];
        var optionTwoId = cardsChosenId[1];

        //Duplo clique na mesma carta
        if (optionOneId == optionTwoId) {
            cards[optionOneId].setAttribute('src', 'images/card.png');
            cards[optionTwoId].setAttribute('src', 'images/card.png');
            contador++;
            alert("Para de ser ladrão")
        }
        //Formando parzim
        else if (cardsChosen[0] == cardsChosen[1]) {
            alert("Formou parzim");
            cards[optionOneId].setAttribute('src', 'images/white.png');
            cards[optionTwoId].setAttribute('src', 'images/white.png');
            cards[optionOneId].removeEventListener('click', flipCard);
            cards[optionTwoId].removeEventListener('click', flipCard);
            pares.push(cardsChosen);
        }
        //Não formou par
        else {
            cards[optionOneId].setAttribute('src', 'images/card.png');
            cards[optionTwoId].setAttribute('src', 'images/card.png');
            contador++;
            alert("Errou, otário");
        }
        cardsChosen = [];
        cardsChosenId = [];
        resultDisplay.textContent = pares.length;
        contagi.textContent = contador;
        if (pares.length == cardArray.length / 2) {
            resultDisplay.textContent = "Parabéns, você nao é tão imbecil";
        }
    }

    //Virando cards
    function flipCard() {
        var cardId = this.getAttribute('data-id');
        cardsChosen.push(cardArray[cardId].name);
        cardsChosenId.push(cardId);
        this.setAttribute('src', cardArray[cardId].img);
        if (cardsChosen.length == 2) {
            setTimeout(checkforMatch, 200);
        }
    }


    createBoard();







})