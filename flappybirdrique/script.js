const canvas = document.getElementById("canvas");
const ctx = canvas.getContext("2d");

//CARREGANDO O BOTAO
const iniciarButton = document.getElementById("iniciar-button")

//CONFIGURANDO O BOTAO
const rect = canvas.getBoundingClientRect();

iniciarButton.style.position = "absolute";
iniciarButton.style.left = "50%";
iniciarButton.style.top = "50%";
iniciarButton.style.transform = "translate(-50%, -50%)";

// CARREGANDO IMAGENS
const bird = new Image();
bird.src = "images/bird.png";
const bg = new Image();
bg.src = "images/bg.png";
const chao = new Image();
chao.src = "images/chao.png"
const canocima = new Image();
canocima.src = "images/canocima.png";
const canobaixo = new Image();
canobaixo.src = "images/canobaixo.png"

// VARIÁVEIS

const eec = 100;
let constant;
let bx = 33;
let by = 200;
let sentidograv = 1;
let gravidade = 180;
let scoreDisplay = 0;
let scoreMesmoCano = [false];
const cano = [];
const folgax = 6;
const folgay = 7;
const birdheight = 26;
const birdwidth = 38;
const cimaheight = 242;
const cimawidth = 52;
const baixoheight = 378;
const baixowidth = 52;
let velocano = 60;

cano[0] = {
    x: canvas.width,
    y: 0,
    novoCriado: false
};

// CARREGANDO SONS
const fly = new Audio();
fly.src = "sounds/fly.mp3";
const scoresound = new Audio();
scoresound.src = "sounds/score.mp3";
const wundebar = new Audio();
wundebar.src = "sounds/wunderbar.mp3";
const bg_sound = new Audio();
bg_sound.src = "sounds/bg_sound.mp3";


// CAPTURA DE TECLA
document.addEventListener("keydown", voa);
document.addEventListener("pointerdown", voa);


// FUNÇÃO PARA VOAR
function voa() {
    fly.play();
    sentidograv = -sentidograv;
};

//FUNÇÃO PRA TOCAR MUSICA
function wundebarplay() {
    wundebar.play();
};

//FUNÇÃO PARA REPRODUZIR SOM BG
function bgsoundplay() {
    bg_sound.play();
};


// INICIAR O JOGO COM BOTAO
iniciarButton.addEventListener("click", jogo);

// INICIAR O JOGO COM ESPAÇO
document.addEventListener("keydown", iniciarComEspaco);


function iniciarComEspaco(event) {
    if (event.code === "Space") {
        jogo();

    }
}

let ultimoTempo = 0;


function resetJogo() {
    sentidograv = 1;
    gravidade = 180;
    velocano = 60;
    bx = 33;
    by = 200;
    i = 0;
    ultimoTempo = 0;
    scoreDisplay = 0;
    scoreMesmoCano.splice(0);
    scoreMesmoCano.push(false);
    cano.splice(0);
    cano.push({
        x: canvas.width,
        y: 0,
        novoCriado: false
    });
    document.addEventListener("keydown", iniciarComEspaco);
    iniciarButton.style.display = "block";
    bgsoundplay.pause();
    bgsoundplay.currentTime = 0;
};

function jogo(tempoAtual) {
    bgsoundplay();
    
    wundebar.pause();
    wundebar.currentTime = 0;

    if (typeof tempoAtual !== "number") {
        tempoAtual = performance.now();
    };

    if (!ultimoTempo) {
        ultimoTempo = tempoAtual;
    };

    let deltaTime = (tempoAtual - ultimoTempo) / 1000;

    ultimoTempo = tempoAtual;

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
        cano[i].x = cano[i].x - velocano * deltaTime;

        //CRIAR NOVOS CANOS
        if (cano[i].x <= 125 && !cano[i].novoCriado) {
            cano[i].novoCriado = true;

            cano.push({
                x: canvas.width,
                y: Math.floor(Math.random() * cimaheight) - cimaheight,
                novoCriado: false
            });
        }
        // PASSARO ENTRE AS BORDAS DO CANO
        if (bx + birdwidth >= cano[i].x + folgax && bx + folgax <= cano[i].x + cimawidth
            // PASSARO COLIDIU COM O CANO DE CIMA OU COM O CANO DE BAIXO
            && (by + folgay <= cano[i].y + cimaheight || by + birdheight >= cano[i].y + constant + folgay)) {
            wundebar.play();
            resetJogo();
            return
        }

        if ((by + birdheight) >= (canvas.height - chao.height)) {
            wundebar.play();
            resetJogo();
            return
        }



        //MARCANDO PONTOS
        if (cano[i].x <= 5 && cano[i].x > 1 && scoreMesmoCano[i] == false) {
            scoreDisplay = scoreDisplay + 1;
            scoreMesmoCano[i] = true;
            scoresound.play();
        }

        if (cano[i].x <= 1 && scoreMesmoCano[i] == true) {
            scoreMesmoCano.push(false);
        }

    }

    // POSICIONANDO O CHAO
    ctx.drawImage(chao, 0, canvas.height - chao.height)

    // DESENHANDO O PASSARO
    ctx.drawImage(bird, bx, by, birdwidth, birdheight)
    by += sentidograv * gravidade * deltaTime;

    // CRIANDO O PLACAR;
    ctx.fillStyle = "#000";
    ctx.font = "20px Verdana";
    ctx.fillText("Placar: " + scoreDisplay, 10, 40);

    requestAnimationFrame(jogo);
};
