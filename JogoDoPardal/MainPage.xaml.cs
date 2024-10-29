


namespace JogoDoPardal;

public partial class MainPage : ContentPage
{

	const int gravidade = 8; //pixel
	const int tempoEntreFrames = 30; //ms
	bool estaMorto = false;
	int gravidadecano = 15;
	double alturaJanela = 0;
	double larguraJanela = 0;
	double alturaDoCano = 270;
	const int maxTempPulando = 1;
	int tempoPulando = 6;
	bool estaPulando = false;
	const int forcaPulo = 100;
	const int aberturMinima = 100;
	int score = 0;


	public MainPage()
	{
		InitializeComponent();
	}

	bool VerificaColisaoCanoCima()
	{
		var posHney = (larguraJanela/2) - (ney.WidthRequest/2);
		var posVney = (alturaJanela/2) - (ney.HeightRequest/2);
		if (posHney >= Math.Abs(canocima.TranslationX) - canocima.WidthRequest &&
			posHney <= Math.Abs(canocima.TranslationX) + canocima.WidthRequest &&
			posVney <= canocima.HeightRequest + canocima.TranslationY)
			{
				return true;
			}
			else
			{
				return false;
			}
	}

	bool VerificaColisaoCanoBaixo()
	{
		var posHpardal = (larguraJanela/2) - (ney.WidthRequest/2);
		var posVpardal = (alturaJanela/2) + (ney.HeightRequest/2) + ney.TranslationY;
		var yMaxCano = ney.HeightRequest + canocima.TranslationY + aberturMinima;
		if (posHpardal >= Math.Abs(canobaixo.TranslationX) - canobaixo.WidthRequest &&
			posHpardal <= Math.Abs(canobaixo.AnchorX) + canobaixo.WidthRequest &&
			posVpardal >= yMaxCano)
			{
				return true;	
			}
			else
			{
				return false;
			}
	}


	bool VerificaColisaoTeto()
	{
		var minY = - alturaJanela/2;
		if (ney.TranslationY <= minY)
			return true;
		else 
			return false;
	}

	bool VerificaColisaoChao()
	{
		var maxY = alturaJanela/2;
		if (ney.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VericaColisao()
	{
		
		if (VerificaColisaoChao() || 
			VerificaColisaoTeto() ||
			VerificaColisaoCanoBaixo() ||
			VerificaColisaoCanoCima())
			
			return true;
			else
		    return false;
	}


	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
	}

	void AplicaGravidade()
	{
		ney.TranslationY += gravidade;
	}
 
	void AplicaSubida()
    {
        ney.TranslationY -= forcaPulo;
		tempoPulando ++;
		if (tempoPulando >= maxTempPulando)
		{
			estaPulando=false;
			tempoPulando=0;
		}
    }
	
	async Task Desenhar()
	{
		while(!estaMorto)
		{   
			if(estaPulando)
				AplicaSubida();
				
			else
				AplicaGravidade();
				CanoFrente();
			if (VericaColisao())
			{ 
				estaMorto = true;
				gameover.IsVisible=true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}

	private void NeySobe(object sender, EventArgs e)
	{
		estaPulando = true;
	}

	void CanoFrente()
	{ 
		canocima.TranslationX -= gravidadecano;
		canobaixo.TranslationX -= gravidadecano;
		if(canobaixo.TranslationX <= -larguraJanela)
		{
			canobaixo.TranslationX = 30;
			canocima.TranslationX = 30;
			var alturaMax = -100;
			var alturaMin = -canobaixo.HeightRequest;
			canocima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			canobaixo.TranslationY = canocima.TranslationY + aberturMinima + canobaixo.HeightRequest;
			score++;
			if(score % 2 == 0)
			{
				gravidadecano++;
			}
			labelscore.Text="Canos:" + score.ToString("D3");
			labelfrase.Text ="VOCÊ PASSOU POR:" + score.ToString("D3") + "CANOS";  
		}
		
	}


	
	void GameOver(object sender, EventArgs e)
	{
		gameover.IsVisible=false;
		Inicializar();
		canobaixo.TranslationX = -larguraJanela;
		canocima.TranslationX = -larguraJanela;
		Desenhar();
	}	

	void Inicializar()
	{
		estaMorto = false;
		ney.TranslationY = 0;
		canocima.TranslationX =- larguraJanela;
		canobaixo.TranslationX =- larguraJanela;
		ney.TranslationX = 0;
		ney.TranslationY = 0;
		score = 0;
		CanoFrente();
	}

}
