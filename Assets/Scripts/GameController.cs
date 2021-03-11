using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public float interval;
    public float velocity;
    public float missingTargetSize;
    public List<GameObject> upperTeeth;

    private Camera _camera;
    private float _lastLaunchedTime = 0.0f;
    private Random _rand;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _rand = new Random();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
    }

    public void LaunchBulletIfNeeded(Vector3 targetPosition)
    {
        if (Time.time < _lastLaunchedTime + interval) return;

        // missingTargetSize の範囲だけランダムにずらす
        var missR = Math.Pow(_rand.NextDouble(), 2) * this.missingTargetSize;
        var missAngle = _rand.NextDouble() * Math.PI * 2;
        var z = System.Numerics.Complex.FromPolarCoordinates(missR, missAngle);
        var missVector = new Vector3((float) z.Real, (float) z.Imaginary, 0);

        LaunchBullet(targetPosition + missVector);
        _lastLaunchedTime = Time.time;
    }

    private void LaunchBullet(Vector3 targetPosition)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Bullet");
        var bullet = Instantiate(prefab);
        bullet.transform.position = _camera.transform.position - new Vector3(0, .5f, 0);

        var rb = bullet.GetComponent<Rigidbody>();
        var force = GameController.CalculateForceToCollide(bullet.transform.position, targetPosition, velocity);
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    private static Vector3 CalculateForceToCollide(Vector3 initPosition, Vector3 targetPosition, float velocity)
    {
        var vector = targetPosition - initPosition;
        var hVector = new Vector3(vector.x, 0, vector.z);

        var distX = (initPosition - new Vector3(targetPosition.x, initPosition.y, targetPosition.z)).magnitude;
        var targetTan = (targetPosition.y - initPosition.y) / hVector.magnitude;
        var angleToTarget = Math.Atan(targetTan);

        // 上に+45度が一番遠くまで投げられる
        var okAngle = Math.Atan(Math.Tan(angleToTarget) + 1.0);
        // まっすぐ投げたら届かない
        var ngAngle = angleToTarget;
        // 二分探索でちょうど当たる位置を探す
        while (Math.Abs(ngAngle - okAngle) > 1e-8)
        {
            var midAngle = (ngAngle + okAngle) / 2.0;
            // http://www.daiichi-g.co.jp/rika/subtextbook/data/46658/46658page14-19.pdf
            var g = Math.Abs(Physics.gravity.y);
            var a = -g / (2 * Math.Pow(velocity, 2) * Math.Pow(Math.Cos(midAngle), 2));
            var b = Math.Tan(midAngle);
            // 必要な角度を目標に傾ける (必要な角度分、下に投げたことにする)
            b -= targetTan;
            // ax^2 + bx = 0 となる x (!= 0)
            var x = -b / a;
            if (x >= distX)
                okAngle = midAngle;
            else
                ngAngle = midAngle;
        }

        var forceVector = new Vector3(hVector.x, (float) Math.Tan(okAngle) * hVector.magnitude, hVector.z);
        // abs(forceVector) == velocity
        forceVector *= velocity / forceVector.magnitude;
        return forceVector;
    }
}