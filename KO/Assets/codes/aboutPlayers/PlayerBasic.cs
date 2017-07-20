using UnityEngine;
using System.Collections;

public class PlayerBasic : MonoBehaviour {

	//游戏人物属性类和游戏人物的数值操作类
	//有关最根本游戏的东西放在这里
	//最基本的计算方法处理

	//给出参考数值用于测试
	//究极人物类，完全就是属性和方法的记录脚本名称
	[HideInInspector]
	public string headPictureName;//人物头像的
	public string ActerName;//这个人物的名称

	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public bool isAlive=true;//是否生存，默认一定是存活的，除非死了

	//最基本的属性生命法力和名字
	public float ActerHpMax=1000f;//这个人物的生命上限
	public float ActerHp=1000f;//这个人物当前的生命值
	public float ActerSpMax=500f;//这个人物的法力上限
	public float ActerSp=200f;//这个人物当前的法力值
	public float ActerHpUp=0.5f;//人物生命恢复
	public float ActerSpUp=0.5f;//人物法力回复

	//特殊战斗属性
	public float ActerSuperBaldePercent=0f;//这个人物的暴击率
	public float ActerSuperBaldeAdder=2f;//暴击时伤害的倍数
	
	public float ActerMissPercent=0f;//这个人物的闪避率
	
	public float ActerShielderPercent=0f;//这个人物的格挡率
	public float ActerShielderDamageMiuns=1;//格挡住的伤害值
	public float ActerShielderDamageMiunsPercent=0.1f;//格挡住的伤害百分比 (先计算固定格挡，然后计算百分比格挡)
	
	//物理战斗属性
	public float ActerWuliDamage=25f;//这个人物的物理攻击力
	public float ActerWuliReDamage=0f;//这个人物的物理反伤
	public float ActerWuliIner=0f;//这个人物的固定物理穿透
	public float ActerWuliInerPercent=0f;//这个人物百分比穿透  （先计算固定穿透，然后计算百分比穿透）
	
	//物理防御属性
	public float ActerWuliShield=150f;//这个人物的物理护甲

	//生命吸取属性
	public float ActerHpSuck=0f;//人物的固定生命偷取值
	public float ActerHpSuckPercent=0f;//根据所造成伤害的百分比生命吸取
	
	//法力吸取属性
	public float ActerSpSuck=0f;//人物的固定的法力偷取
	public float ActerSpSuckPercent=0f;//根据所造成伤害的百分比法力偷取


	//额外战斗属性
	public float ActerDamageAdderPercent=0;//额外百分比伤害
	public float ActerDamageAdder=0;//额外真实加成
	//上面这些全都要放在RPC方法里面各种更新
	//人物等级提升后加成

	public float ActerMoveSpeedPercent = 1f;//移动速度百分比，在移动的时候会有这个速度百分比加成

	public float ActerShieldHp = 0;//护盾的生命值

	//接下来是一些私有的战斗属性备份，用于计算值（作为例如护甲值提升10%这种的参数值计算）
	//因为是私有方法，所以还要给出获取这个值和修改这个值的方法
	//总体上讲，这些值是战斗属性的备份值，当有特殊计算方法的时候作为参数计算更新战斗属性
	//例如 战斗属性 = 备份值 *1.1f
	//顺带一提，之所以使用私有方法是因为不想让共有属性表太长，此外这些私有只会在特殊情况之下服务器才会使用

	//最基本的属性生命法力和名字
	public float CActerHpMax=1000f;//这个人物的生命上限
	public  float CActerSpMax=500f;//这个人物的法力上限
	public  float CActerHpUp=0.5f;//人物生命恢复
	public  float CActerSpUp=0.5f;//人物法力回复

	//特殊战斗属性
	public  float CActerSuperBaldePercent=0f;//这个人物的暴击率
	public  float CActerSuperBaldeAdder=2f;//暴击时伤害的倍数
		
	public  float CActerMissPercent=0f;//这个人物的闪避率

	public  float CActerShielderPercent=0f;//这个人物的格挡率
	public  float CActerShielderDamageMiuns=1;//格挡住的伤害值
	public  float CActerShielderDamageMiunsPercent=0.1f;//格挡住的伤害百分比 (先计算固定格挡，然后计算百分比格挡)
	

	//物理战斗属性
	public  float CActerWuliDamage=25f;//这个人物的物理攻击力
	public  float CActerWuliReDamage=0f;//这个人物的物理反伤
	public  float CActerWuliIner=0f;//这个人物的固定物理穿透
	public  float CActerWuliInerPercent=0f;//这个人物百分比穿透  （先计算固定穿透，然后计算百分比穿透）


	//物理防御属性
	public float CActerWuliShield=150f;//这个人物的物理护甲

	//生命吸取属性
	public  float CActerHpSuck=0f;//人物的固定生命偷取值
	public  float CActerHpSuckPercent=0f;//根据所造成伤害的百分比生命吸取

	public float CActerMoveSpeedPercent = 1f;//移动速度百分比，在移动的时候会有这个速度百分比加成

	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public  float extraDamageForAnimation = 0;//设置为共有是为了传参数的时候方便，但是这个参数是不能够被主动在面板上设定的


///////////////////////////////////////////////////////////////////////////////////////////////////////	
//下面是游戏计算中的临时变量
	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	//这个参数非常重要，决定着操纵这个人物的固定按键设定（游戏手柄）
	public int playerIndex = 0;//游戏人物的下标,因为是二人对战格斗，只有可能是0,1（在初始化的时候才会进行唯一一次的设定）


	private effectBasic[] effects;
	private GUIStyle GUIShowStyle;//GUI显示的人物当前生命值
	private bool isStarted =false;//是否已经开启

	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public attackLinkController theAnimationController;
	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public string conNameToEMY ;//用于记录连招给出的额外buff
	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public string conNameToSELF;//用于记录连招给出的额外buff
	private float conNameCoolingTime = 0.2f;//如果这个特效脚本在1.5秒内还没有被使用，就认为无效
	//（否则准备一个脚本招式，然后等半天莫名其妙生效了不是好事）
	private float conNameCoolingTimeMax = 1.5f;

	//一个游戏人物的所有的武器拥有统一的冷却时间
	//冷却时间越短，造成伤害的时间间隔也就越短
	[HideInInspector]
	public  float timerForHit = 0.2f;
	[HideInInspector]
	public   float timerForHitMax = 0.2f;
	[HideInInspector]
	public  bool canHit =true;
 
	[HideInInspector]
	public  int winCountNow =0 ;//这个人物已经胜利的次数
	//有了静态的转换方法之后这个参数就不需要指定了，这样耦合性更低
	public float TimePercent = 1;//武器冷却时间百分比，不同武器根据其动作可能会有不同的冷却时间
	//(这个属性在使用动画关键帧的时候被暂时弃用，但这个思路还是有的)
	public float theAttackAreaLength;//攻击范围（非常重要，同时这个是简化版本的每一种攻击招式分开计算范围的方式）
	private float DamageRead = 0;//记录已经收到的伤害，如果收到的伤害累积到一定数量就要播放受到攻击的动画
	//音效播放器
	public audioPlayer theAudioPlayer;//自己定义的音频播放器组件
	//值得注意的是声音的播放是在attackLink里面调用的，在这里保留引用是为了减少获取的次数
 

	public void addDamageRead(float damage=0)//有些技能是带有额外伤害的，这个伤害也需要统计到这里面去
	{
		DamageRead += damage;
	}

	public string getPlayerInformation()
	{
		string information = "";
		//information += "=======" + this.ActerName+"=======\n\n";
		information += "生命值上限 "+(int)this.ActerHpMax+"    ";
		information += "斗气值上限 "+(int)this.ActerSpMax+"\n";
		information += "生命恢复 " + this.ActerHpUp .ToString ("f2")+"/秒   ";
		information += "斗气回复 " + this.ActerSpUp .ToString ("f2") + "/秒\n";
		information += "护甲 " + this.ActerWuliShield .ToString ("f1")+"    ";
		information += "伤害 " + this.ActerWuliDamage .ToString("f1")+"    ";
		information += "反伤 "+this.ActerWuliReDamage.ToString("f1")+"";
		return information;
	}

	public string getPlayerInformationExtra()
	{
		string information = "";
		information += "暴击率 "+(this.ActerSuperBaldePercent *100).ToString("f1")+"%    ";
		information += "暴击伤害加成 "+(this.ActerSuperBaldeAdder*100).ToString("f1")+"%\n";
		information += "闪避率 "+(this.ActerMissPercent *100).ToString("f0")+"%\n";
		information += "格挡率 "+(this.ActerShielderPercent *100).ToString("f0")+"%    ";
		information += "格挡真实伤害减免 " + this.ActerShielderDamageMiuns.ToString ("f1")+"    ";
		information += "格挡百分比伤害减免 " + (this. ActerShielderDamageMiunsPercent *100).ToString("f0")+"%\n";

		information += "真实穿透 "+this. ActerWuliIner.ToString("f1")+"    ";
		information += "百分比穿透 "+(this.ActerWuliInerPercent*100).ToString("f1")+"%\n";
		information += "真实生命偷取 "+this.ActerHpSuck.ToString("f1")+"    ";
		information += "伤害/生命转化 "+(this.ActerHpSuckPercent*100).ToString("f0")+"%\n";
		information += "额外伤害 "+(this.ActerDamageAdderPercent*100).ToString("f1")+"%    ";
		information += "额外伤害加成 "+(this.ActerDamageAdder).ToString("f1")+"\n";
		information += "攻击范围 "+this.theAttackAreaLength*100+"%";

		return information;
	}

	//这个方法返回的仅仅是字符串
	//旧版方法，替代方法在selectSceenController中
	effectBasic[] effectsPE;//这个方法的临时变量
	public string getSkillInformation()
	{
		string  information = "=======" + "特殊技能" +"=======\n(在使用相应招式的时候能够触发的特殊效果 )\n\n";
		attackLink [] theWeapons = this.GetComponentsInChildren<attackLink> ();
		for (int i = 0; i < theWeapons.Length; i++) 
		{
			if (string.IsNullOrEmpty (theWeapons [i].conNameToEMY) == false) 
			{
				this.gameObject.AddComponent ((System.Type.GetType (theWeapons [i].conNameToEMY)));
			}
			if (string.IsNullOrEmpty (theWeapons [i].conNameToSELF) == false) 
			{
				this.gameObject.AddComponent ((System.Type.GetType (theWeapons [i].conNameToSELF)));
			}
		}
		effectsPE  = this.GetComponentsInChildren <effectBasic> ();
		for (int i = 0; i < effectsPE.Length; i++)
		{

			if (effectsPE [i].isBE ())
				information += "<color=#FFFF00>";
			else
				information += "<color=#00FF00>";
			effectsPE [i].Init ();
			information += effectsPE [i].getInformation ()+"</color>\n\n";
			effectsPE [i].effectDestoryExtra ();
			effectsPE [i].effectDestory ();
			Destroy (this.GetComponent (effectsPE [i].GetType()),0.004f);
		}
		return information;
	}

	//这个方法返回的仅仅是字符串
	//旧版方法，替代方法在selectSceenController中
	attackLink  [] theLinks ;//这个方法的临时变量
	public string getAttackLinkInformation()
	{
		string information = "";
		//information+="=======" + "招式" +"=======\n\n"; 
		theLinks = this.GetComponentsInChildren<attackLink> ();
		for (int i = 0; i < theLinks.Length; i++)
		{
			information += theLinks [i].getInformation ()+"\n";
		}
		return information;
	}

	//初始化备份的方法，只是在最开始的时候调用一次
	void startCValues()
	{
		//最基本的属性生命法力和名字
		CActerHpMax=ActerHpMax;//这个人物的生命上限
		CActerSpMax=ActerSpMax;//这个人物的法力上限
		CActerHpUp=ActerHpUp;//人物生命恢复
		CActerSpUp=ActerSpUp;//人物法力回复
		
		//特殊战斗属性
		CActerSuperBaldePercent=ActerSuperBaldePercent;//这个人物的暴击率
		CActerSuperBaldeAdder=ActerSuperBaldeAdder;//暴击时伤害的倍数

		CActerMissPercent = ActerMissPercent;//这个人物的闪避率
		CActerShielderPercent=ActerShielderPercent;//这个人物的格挡率
		CActerShielderDamageMiuns=ActerShielderDamageMiuns;//格挡住的伤害值
		CActerShielderDamageMiunsPercent=ActerShielderDamageMiunsPercent;//格挡住的伤害百分比 (先计算固定格挡，然后计算百分比格挡)
		
		
		//物理战斗属性
		CActerWuliDamage=ActerWuliDamage;//这个人物的物理攻击力
		CActerWuliReDamage=ActerWuliReDamage;//这个人物的物理反伤
		CActerWuliIner=ActerWuliIner;//这个人物的固定物理穿透
		CActerWuliInerPercent=ActerWuliInerPercent;//这个人物百分比穿透  （先计算固定穿透，然后计算百分比穿透）
		
		//物理防御属性
		CActerWuliShield=ActerWuliShield;//这个人物的物理护甲


		//生命吸取属性
		CActerHpSuck=ActerHpSuck;//人物的固定生命偷取值
		CActerHpSuckPercent=ActerHpSuckPercent;//根据所造成伤害的百分比生命吸取

		//移动速度百分比
		CActerMoveSpeedPercent = ActerMoveSpeedPercent;//人物的移动速度百分比

	}

	//真正的伤害方法组
	//所有的伤害都在这里计算
	//进行攻击方法组

	/**********************************************************************************/
	//在攻击的起手阶段触发
	public void onPlayattackActions()
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++) 
		{
			effects [i].onAttackAction ();
		}
	}

	//在攻击命中的时候触发
	public void OnAttack(PlayerBasic thePlayerAim ,float extraDamage = 0 ,bool isSimple =false,bool isExtraAttack = false)
	{
		//isExtraAttack表现为不用什么特殊动作直接造成伤害的条件
		//在这里似乎多做了一次判断
		//判断原因在于原先的攻击做法是连续的，而现在的攻击是离散的
		if (theAnimationController.canChangeToNextAttack() == false || isExtraAttack) 
		{
			this.theAudioPlayer.playAttackSound ();//播放攻击音效
			float damage = 0;
			if(isSimple)//如果只是简单地真实伤害
				damage  =extraDamage;
			else
				damage = getTrueDamage (thePlayerAim, extraDamage+extraDamageForAnimation);
			
			thePlayerAim.OnBeAttack (damage);

			effects = this.GetComponentsInChildren<effectBasic> ();
			for (int i = 0; i < effects.Length; i++) 
			{
				effects [i].OnAttack ();
				effects [i].OnAttack (thePlayerAim);
			}
			extraDamageForAnimation = 0;
		}
	}

	public void OnAttackExtra(PlayerBasic thePlayerAim , bool isSimple =false )
	{
		this.theAudioPlayer.playAttackSound ();//播放攻击音效
         //不太按规矩来的伤害计算，例如龟派气功
			float damage = 0;
				damage = getTrueDamage (thePlayerAim,  extraDamageForAnimation);

			thePlayerAim.OnBeAttack (damage);

			effects = this.GetComponentsInChildren<effectBasic> ();
			for (int i = 0; i < effects.Length; i++) 
			{
				effects [i].OnAttack ();
				effects [i].OnAttack (thePlayerAim);
			    effects [i].OnAttack (thePlayerAim,damage);
			}
			extraDamageForAnimation = 0;
	}

	//有些攻击不想触发特效也不希望靠判断防止递归，就调用下面这两个方法
	public void OnAttackWithoutEffect(PlayerBasic thePlayerAim ,float extraDamage = 0 ,bool isSimple =false,bool isExtraAttack = false)
	{
		//isExtraAttack表现为不用什么特殊动作直接造成伤害的条件
		if (theAnimationController.canChangeToNextAttack() == false ||isExtraAttack) 
		{
			this.theAudioPlayer.playAttackSound ();//播放攻击音效
			float damage = 0;
			if(isSimple)//如果只是简单地真实伤害
				damage  =extraDamage;
			else
				damage = getTrueDamage (thePlayerAim, extraDamage+extraDamageForAnimation);

			thePlayerAim.OnBeAttack (damage);

			extraDamageForAnimation = 0;
		}
	}

	public void OnAttackExtraWithoutEffect(PlayerBasic thePlayerAim , bool isSimple =false )
	{
		this.theAudioPlayer.playAttackSound ();//播放攻击音效
		//不太按规矩来的伤害计算，例如龟派气功
		float damage = 0;
		damage = getTrueDamage (thePlayerAim,  extraDamageForAnimation);

		thePlayerAim.OnBeAttack (damage);

		extraDamageForAnimation = 0;
	}


	public void OnBeAttack(float damage)
	{
		if (this.isAlive)//只有在活着的时候才可以被攻击
		{
			if (systemValues.canBeAttack (this.theAnimationController.theAnimator) == false)
				return;//如果处于一些特殊状态下，无视攻击
				
			DamageRead += damage;//累计伤害

			//如果护盾比较厚，就吸收所有的伤害
			if (ActerShieldHp > damage)
			{
				ActerShieldHp -= damage;
				damage = 0;
			} 
			else//如果护盾不够厚，护盾消失
			{
				damage -= ActerShieldHp;
				ActerShieldHp = 0;
			}
			this.ActerHp -= damage;
			effects = this.GetComponentsInChildren<effectBasic> ();
			for (int i = 0; i < effects.Length; i++)
				effects [i].OnBeAttack (damage);
			//如果收受到了较重的伤害，那么就取消当前的攻击动作，强制转为受到攻击的动作，并且攻击力转化为0
			//此外beHit状态已经在systemValues里面标注为无法造成伤害的状态之一
			//在这里将额外攻击力取消掉是因为这个是强制转换的，可能会有动作做到一半的情况，也许会有额外的伤害误差
			//此外，重击伤害暂定为一次受到最大生命值5%以上的伤害，但这是有误差的，因为有些攻击时存在效果伤害的
			//初步的思路是在效果中额外添加受到伤害的动作使之成为一种必然发生的动画

			if (damage > this.ActerHpMax * 0.05f || DamageRead > this.ActerHpMax*0.10f) 
			{ //伤害范围需要界定
				extraDamageForAnimation = 0;//消除自身攻击效果
				DamageRead  = 0;//重新统计伤害
				this.theAnimationController.playFaint ();
				/*****************************************截图的临时安放位置，千万注意************************************************/
				if(systemValues .canMAkePicture)//对于较低配置的目标计算机，关闭这个就可以节约比较多的计算
				StartCoroutine (savePic());//截图的临时处理，其实真正的截图更应该是精彩瞬间的判断，或许会需要图像识别等等技术
			}
		}
	}

	IEnumerator savePic()//截图
	{
		yield return new WaitForEndOfFrame ();
		GameObject.Find ("/gameController").GetComponent <pictureMaking>().savePicture();
	}

	float getTrueDamage(PlayerBasic thePlayerAim, float extraDamage =0)//真正的计算伤害的方法，这个方法被“攻击”的时候调参数为攻击者经过计算的伤害
	{
		if (Random.value < thePlayerAim.ActerMissPercent)
			return 0f;//伤害整个被无视，也就是被闪避了
		
		float damageMake = this.ActerWuliDamage - this. ActerWuliIner +extraDamage;
		float hujiaGet = thePlayerAim .ActerWuliShield*(1- this.ActerWuliInerPercent);
		damageMake *= 1 - (hujiaGet / 1500);//固定1500就是防御的上界
		damageMake += this. ActerWuliIner;
		damageMake = damageMake * (1 + this.ActerDamageAdderPercent) + ActerDamageAdder;

		if (Random.value < thePlayerAim.ActerShielderPercent)//目标发生格挡
		{
			damageMake -= thePlayerAim.ActerShielderDamageMiuns;
			damageMake *= (1- thePlayerAim.ActerShielderDamageMiunsPercent);
		}

		if (Random.value < ActerSuperBaldePercent)//攻击者暴击
		{
			damageMake *= ActerSuperBaldeAdder;
		}
		float hpChanger = damageMake * ActerHpSuckPercent + ActerHpSuck - thePlayerAim.ActerWuliReDamage*(1- this.ActerWuliShield/1500);
		this.ActerHp += hpChanger;//关于反伤和吸血的制作比较简单
		return damageMake;
	}
	/***********************************************************************************/

	public void OnUpdate()//在更新的时候能够做到的额外计算（这个是有冷却时间的，初步设定为每一秒计算一次）
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++)
			effects [i].effectOnUpdate ();
	}

	public void OnHpToMax()//每一次恢复到满血的时候调用一次
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++)
			effects [i].OnHpTowardHpMax();
	}

	public void OnSpToMax()//每一次恢复到满血的时候调用一次
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++)
			effects [i].OnSpTowardSpMax();
	}

	public void OnHpUp()//每一次恢复生命都会调用
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++)
			effects [i].OnHpUp();
	}
	public void OnSpUp()//每一次恢复到满血的时候调用一次
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++)
			effects [i].OnSpUp();
	}
	public void OnUseSp(float spUse =0)//每一次恢复到满血的时候调用一次
	{
		effects = this.GetComponentsInChildren<effectBasic> ();
		for (int i = 0; i < effects.Length; i++)																																								
			effects [i].OnUseSP(spUse);
	}

	//这个方法用预碰撞检测使用，但如果使用动画的方法事实上就没必要使用了
	public  void flashWeapon()
	{
		if (canHit == false)
		{
			timerForHit -= systemValues.updateTimeWait;//千万注意此处因为没有在update里面调用，所以不推荐用TimedeltaTime这种，会有额外的误差
			if (timerForHit < 0) 
			{
				timerForHit = timerForHitMax*TimePercent;
				canHit = true;
			}
		}
	}

	public void flashConNameTimer()
	{
		conNameCoolingTime = conNameCoolingTimeMax;
	}


 

	/*****************************************************************************************/
	//这个方法每一帧都会调用，刷新任务的属性
	//并没有使用update方法而是使用invoke方法
	//inovke的调用时间间隔由systemValues类进行统一配置
	public void updateValue()
	{
		if (isStarted && isAlive) 
		{
			//默认机制就是每一次恢复每秒钟的生命恢复再检查是否死亡
			if (ActerHp < ActerHpMax) {
				ActerHp += ActerHpUp * systemValues.updateTimeWait;
				OnHpUp ();
				if (ActerHp > ActerHpMax) {
					ActerHp = ActerHpMax;	
					OnHpToMax ();
				}
			}
			if (ActerHp < 0) {
				ActerHp = 0;	
				isAlive = false;
				this.gameObject.tag = "dead";
				if (this.GetComponent <attackLinkController> ())
					this.GetComponent <attackLinkController> ().playDead ();//因为仅此一次，所以没必要事先就保留引用
			      else
					Destroy (this.gameObject, 1.5f);

			}
			if (ActerHp > ActerHpMax) {
				ActerHp = ActerHpMax; //最后一个修正
			}

			//////////////////////////////////////////////
			if (ActerSp < ActerSpMax) {
				ActerSp += ActerSpUp * systemValues.updateTimeWait;
				OnSpUp ();
				if (ActerSp > ActerSpMax) {
					ActerSp = ActerSpMax;	
					OnSpToMax ();
				}
			}
			if (ActerSp < 0) {
				ActerSp = 0;	
			}
			if (ActerSp > ActerSpMax) {
				ActerSp = ActerSpMax;
			}
			//此方法为物理碰撞的方法进行，但是对于动画的方法是一个阻碍，暂且注释
			//flashWeapon ();//所有的武器共有冷却时间，攻击之后一定时间之内攻击无法命中


			if (!string.IsNullOrEmpty (conNameToEMY) || !string.IsNullOrEmpty (conNameToSELF)) 
			{
				conNameCoolingTime -= systemValues.updateTimeWait;
				if (conNameCoolingTime <= 0)
				{
					conNameCoolingTime = conNameCoolingTimeMax;
					conNameToEMY = "";
					conNameToSELF = "";
				}
			}
			else
				conNameCoolingTime = conNameCoolingTimeMax;//此处多次空转赋值实际上是一个很大的浪费

		
		}
	}


	public void makeStart()//初始化方法，由总控单元统一进行初始化
	{
		startCValues();//因为只有服务器上面的英雄才会使用这些参数
		InvokeRepeating("updateValue",0,systemValues .updateTimeWait);//每隔一秒钟计算额外的计算脚本
		InvokeRepeating("OnUpdate",0,1f);//每隔一秒钟计算额外的计算脚本
		theAnimationController = this.GetComponentInChildren<attackLinkController>();
		GUIShowStyle=new GUIStyle();
		GUIShowStyle.normal.background = (Texture2D)Resources.Load ("pictures/hpGUI");

		isStarted = true;
		isAlive=true;
	}


	//由于这个类是一个究极的父类，因此具体的工作是不做的，但是留下了调用各种方法的方式木板
	void Start () 
	{
		//这个应该是最先初始化的，因为有一些声音可能需要提前使用
		theAudioPlayer = this.GetComponent <audioPlayer> ();
	}

	void OnGUI()
	{ 
		if (systemValues.isGUIShowHP && GUIShowStyle!=null)
		{
			float roto = Mathf.Clamp ((this.ActerHp / this.ActerHpMax), 0f, 1f);
			Vector2 c = Camera.main.WorldToScreenPoint (new Vector3 (this.transform.position.x, this.transform.position.y + 1.25f, this.transform.position.z - 0.5f));
			GUI.BeginGroup (new Rect (c.x, Screen.height - c.y, 155, 100));
			GUI.Box (new Rect (10, 0, 127, 15), "");
			GUI.Box (new Rect (12, 1, 120 * roto, 13), "", GUIShowStyle);
			GUI.EndGroup ();
		}
	}
}
