var canvas = document.getElementById("canvas");
var ctx = canvas.getContext("2d");

//CARREGANDO O BOTAO
var iniciarButton = document.getElementById("iniciar-button")

//CONFIGURANDO O BOTAO
var rect = canvas.getBoundingClientRect();

iniciarButton.style.position = "absolute";
iniciarButton.style.left = "50%";
iniciarButton.style.top = "50%";
iniciarButton.style.transform = "translate(-50%, -50%)";

// CARREGANDO IMAGENS
var bird = new Image();
bird.src = "images/bird.png";
var bg = new Image();
bg.src = "images/bg.png";
var chao = new Image();
chao.src = "images/chao.png"
var canocima = new Image();
canocima.src = "images/canocima.png";
var canobaixo = new Image();
canobaixo.src = "images/canobaixo.png"

// VARIÁVEIS

var eec = 100;
var constant;
var bx = 33;
var by = 200;
var gravidade = 2.3;
var score = 0;
var cano = [];
var folgax = 5;
var folgay = 5;
const birdheight = 26;
const birdwidth = 38;
var cimaheight = 242;
var cimawidth = 52;
var baixoheight = 378;
var baixowidth = 52;
var velocano = 1;

cano[0] = {
    x: canvas.width,
    y: 0
}



// CARREGANDO SONS
var fly = new Audio();
fly.src = "sounds/fly.mp3";
var scoresound = new Audio();
scoresound.src = "sounds/score.mp3";
var wundebar = new Audio();
wundebar.src = "sounds/wunderbar.mp3";
var pretocigano = new Audio();
pretocigano.src = "sounds/preto-cigano.mp3";

// CAPTURA DE TECLA
document.addEventListener("keydown", voa);
document.addEventListener("pointerdown", voa);


// FUNÇÃO PARA VOAR
function voa() {
    fly.play();
    by = by - 38;
}

//FUNÇÃO PRA TOCAR MUSICA
function wundebarplay() {
    wundebar.play();
}


// INICIAR O JOGO COM BOTAO
iniciarButton.addEventListener("click", jogo);

// INICIAR O JOGO COM ESPAÇO
document.addEventListener("keydown", iniciarComEspaco);

function iniciarComEspaco(event) {
    if (event.code === "Space") {
        jogo();

    }
}

function jogo() {
    // POSICIONANDO FUNDO DO JOGO
    ctx.drawImage(bg, 0, 0) // (imagem, posicao x, posicao y)

    //OCULAR BOTAO APÓS INICIAR O JOGO
    iniciarButton.style.display = "none";

    //REMOVER EVENTO DA BARRA DE ESPAÇO
    document.removeEventListener("keydown", iniciarComEspaco);

    //CRIANDO CANOS
    for (let i = 0; i < cano.length; i++) {
        //POSIÇÃO DO CANO DE BAIXO
        constant = cimaheight + eec;

        //POSIÇÃO DO CANO DE CIMA
        ctx.drawImage(canocima, cano[i].x, cano[i].y, cimawidth, cimaheight);

        //CONFIGURANDO O CANO DE BAIXO
        ctx.drawImage(canobaixo, cano[i].x, cano[i].y + constant, baixowidth, baixoheight);

        //MOVIMENTAÇÃO DO CANO
        cano[i].x = cano[i].x - velocano;

        //CRIAR NOVOS CANOS
        if (cano[i].x == 125) {
            cano.push({
                x: canvas.width,
                y: Math.floor(Math.random() * cimaheight) - cimaheight
            })
        }
        // PASSARO ENTRE AS BORDAS DO CANO
        if (bx + birdwidth >= cano[i].x + folgax && bx + folgax <= cano[i].x + cimawidth
            // PASSARO COLIDIU COM O CANO DE CIMA OU COM O CANO DE BAIXO
            && (by + folgay <= cano[i].y + cimaheight || by + birdheight >= cano[i].y + constant + folgay)) {
            pretocigano.play();
            bx = 33;
            by = 200;
            cano = [{
                x: canvas.width,
                y: 0
            }]
            i = 0;
            document.addEventListener("keydown", iniciarComEspaco);
            setTimeout(function () {
                location.reload();
            }, 6000)
        }

        if ((by + birdheight) >= (canvas.height - chao.height)) {
            bx = 33;
            by = 200;
            cano = [{
                x: canvas.width,
                y: 0
            }]
            i = 0;
            document.addEventListener("keydown", iniciarComEspaco);
            wundebar.play();
            gravidade = 0;
            velocano = 0;
            setTimeout(function () {
                location.reload();
            }, 6000)
        }



        //MARCANDO PONTOS
        if (cano[i].x == 5) {
            score = score + 1;
            scoresound.play();
        }

    }

    // POSICIONANDO O CHAO
    ctx.drawImage(chao, 0, canvas.height - chao.height)

    // DESENHANDO O PASSARO
    ctx.drawImage(bird, bx, by, birdwidth, birdheight)
    by += gravidade;

    // CRIANDO O PLACAR;
    ctx.fillStyle = "#000";
    ctx.font = "20px Verdana";
    ctx.fillText("Placar: " + score, 10, 40);

    requestAnimationFrame(jogo);
};
