using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {
	
    [Tooltip("Laser instantiate position")]
	public Transform rayBeginPos; // laser instantiate postion 

    [Tooltip("Laser Hit Layer")]
    public LayerMask enemyHitLayer; // which layer laser should hit 

    [Tooltip("Allocate the The Laser Size")]
    [Range(5,50)]
	public float maxLaserSize; // laser size

    [Range(1,20)]
    public float laserGlowMultiplier;
    [Space]

    [Tooltip("Emitter when laser collide with Enemy/EnemyHitLayer")]
    public GameObject laserHitEmitter; // emitter when laser collide with Enemy/EnemyHitLayer

    [Tooltip("Emitter when laser start firingr")]
    public GameObject laserMeltEmitter;  // emitter when laser start firing 

    [Tooltip("Laser Damage amount for enemy")]
	public int laserDamage; // laser damage amout for enemy 

    [Tooltip("Laser Firing Duration")]
    public float rayDuration; // laser firing duration

    [Tooltip("The Laser Glow Sprite")]
    public Transform laserGlow;

	public float laserSpeed = 1f;

	public float max_angle;

	private DragonControl dc;

    private bool laserOn = false; // switching variable 


	private LineRenderer lineRenderer; 
	private Animator theAnimator; // to animate laser beginning animation 
    private float length = 0; // length of linerender 
    float lerpTime = 1f; // used to lerp 
    private float currentLerpTime = 0; // used to lerp

    GameObject hitParticle = null;  // you can use object pooler here 
    GameObject meltParticle = null; // you can use object pooler here 
    Vector2 endPos;

   // private EnemyHealth theEnemy; // used to cache EnemyHealth component 

    private bool canFire = true;
	private float angle;	// controls the head laser
	private bool facingRight = false;

    void Start(){

        laserGlow.gameObject.SetActive(false); 
		lineRenderer = GetComponent <LineRenderer> ();
		lineRenderer.enabled = false; // at the beginning the linerenderer should disable 
		lineRenderer.sortingOrder = 5; // tthe laser should be visible top of the enemy layer

		theAnimator = GetComponent <Animator> (); // get the animator 
		dc = transform.root.GetComponent<DragonControl>();
    }

    /// <summary>
    ///  get call from animation event 
    /// </summary>
    void activeLaser(){
		laserOn = true;
		GetComponent<AudioSource> ().Play ();

	}

    /// <summary>
    /// deactiavte laser after certain time and deactivate the laser
    /// </summary>
    void deactiveLaser() {
        theAnimator.SetBool("startLaser", false);
        laserOn = false;
		lineRenderer.enabled = false;
		GetComponent<AudioSource> ().Stop ();
    }


	void FixedUpdate() {
        // laser is on 
        if (laserOn) {
			

            float currentLaserSize = maxLaserSize;
            //endPos = new Vector2(rayBeginPos.position.x, transform.position.y + maxLaserSize); // end position of the layer just make sure its out of game screen
            //Vector2 laserDir = Vector2.up; // laser direction 

            // Debug.DrawRay(rayBeginPos.position, laserDir, Color.black);
            //Debug.DrawLine(rayBeginPos.position, endPos, Color.black);     

            lineRenderer.enabled = true;
			lineRenderer.sortingLayerName = "ForeGround";
            //RaycastHit2D hit = Physics2D.Raycast(rayBeginPos.position, laserDir, currentLaserSize, enemyHitLayer); // cast ray
			// detect the position of the ray hitting the ground layer
			RaycastHit2D hit = Physics2D.Raycast (rayBeginPos.position, new Vector2 (angle, -1), currentLaserSize, enemyHitLayer);

            //print(hit.distance);

            if (meltParticle == null) {
                meltParticle = Instantiate(laserMeltEmitter, rayBeginPos.position, Quaternion.identity) as GameObject;
                meltParticle.transform.parent = this.transform;
            }



            // laser hit a physics body 
            if (hit.collider != null) {

                lineRenderer.SetPosition(0, rayBeginPos.position);
                lineRenderer.SetPosition(1, hit.point);

               // hit = laserColGlow(hit);

                // hit emitter 
                if (hitParticle == null) {
                    hitParticle = Instantiate(laserHitEmitter, hit.point, Quaternion.identity) as GameObject;
                }
                if (hitParticle != null) {
                    hitParticle.transform.position = hit.point;
                }

                // give enemy damage
                if (hit.collider.tag == "Player") {
					hit.collider.gameObject.GetComponent<MonkeyControl> ().death (false);
					dc.stopUlti ();
					deactiveLaser ();
                }
				if (!facingRight)
					angle -= Time.deltaTime * laserSpeed;
				else
					angle += Time.deltaTime * laserSpeed;


				if (Mathf.Abs (angle) > max_angle) {
					dc.stopUlti ();
					deactiveLaser ();
				}

            }
            else {
				
                if (hitParticle != null) {
                    Destroy(hitParticle);
                }

                // laser dont collide with any physics body 

                float perc = _lerpLaser();
				Vector2 lerp = Vector2.Lerp(rayBeginPos.position, hit.point, perc * 3f);

                lineRenderer.SetPosition(0, rayBeginPos.position);
                lineRenderer.SetPosition(1, lerp);

              //  laserNotColGlow();
            }
        }
        else if (laserOn == false) {

            if (hitParticle != null) {
                Destroy(hitParticle);
            }

            if (meltParticle != null) {
                Destroy(meltParticle);
            }

            //turnOfLaser(); // turing of

          //  laserGlow.gameObject.SetActive(false);
        }

    } //end update

    
    /// <summary>
    /// this glow effect when laser did not collide with any Enem Object
    /// </summary>
    private void laserNotColGlow() {
        //print("laser dont hit");
        if (!laserGlow.gameObject.activeInHierarchy) {
            laserGlow.gameObject.SetActive(true);
        }

        float perc = _lerpLaser();
        float l = endPos.y + (maxLaserSize * laserGlowMultiplier);

        float lerp = Mathf.Lerp(laserGlow.localScale.y, l, perc);

        laserGlow.localScale = new Vector2(laserGlow.localScale.x, lerp);
    }

    public void FireLaser() {
        // switching laser and player power

        theAnimator.SetBool("startLaser", true);
		if (facingRight)
			angle = 0.2f;
		else
			angle = -0.2f;
		Invoke("activeLaser", 1.5f);

    }


    /// <summary>
    /// this glow effect when laser collide with any Enem Object
    /// </summary>
    private RaycastHit2D laserColGlow(RaycastHit2D hit) {
        if (!laserGlow.gameObject.activeInHierarchy) {
            laserGlow.gameObject.SetActive(true);
        }
        laserGlow.localScale = new Vector2(laserGlow.localScale.x, hit.distance * 4.35f);
        return hit;
    }

    void enableFiring() {
        canFire = true;
    }

    /// <summary>
    /// this method used to lerp Vector
    /// </summary>
    /// <returns>
    /// value between 0 to 1
    /// </returns>
    private float _lerpLaser() {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        return perc;
    }


    /// <summary>
    /// turn of laser smoothly 
    /// </summary>
    void  turnOfLaser() {

        Vector2 startPos = lineRenderer.GetPosition(0);
        Vector2 endPos = lineRenderer.GetPosition(1);

        length = (endPos - startPos).magnitude;

        float perc = _lerpLaser();

        Vector2 lerp =  Vector2.Lerp(endPos, startPos, perc);

        if (length > 0.3f) {
            lineRenderer.SetPosition(1, lerp);
        }
        else {
           lineRenderer.enabled = false;
            currentLerpTime = 0;
         }

    }

	public void setFacing(bool val){
		facingRight = val;
	}
    
}
