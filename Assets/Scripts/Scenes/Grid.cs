using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : SingletonComponent<Grid> {

    public float gridBrightNess = 1f;

    public Vector2 gridCenter;
    public List<Vector2> positions = new List<Vector2>();
    public Vector3[] squaresRotationSpeed = new Vector3[3];
    public float[] squaresWobble = new float[3];

    public bool sendData = false;



    public int rowsCount, columnsCount;

    public float pingDuration = 1f;
    public float pingSize = 0.01f;
    public float pingHOLD = 2f;
    public float holdPointsMultiplier = 0.02f;

    public float scanRange = .25f;


    // public Vector3[] rotaions = new Vector3[3];
    public float glitchMultiplier = .25f;

    public float gridCellSize = 1f;

    void Start()
    {
        sendData = true;
        OnValidate();
        //  StartCoroutine(PingDot(2));
        //StartCoroutine(PingDot(3));
    }

    IEnumerator StartPointsAtLine()
    {
        int column = Random.Range(0, columnsCount - 1);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(PingDot(i, column, i));
            yield return new WaitForSeconds(i * .1f);
        }
    }



    // Update is called once per frame
    void Update () { 
        if(Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(StartPointsAtLine());
        // StartCoroutine(GlitchRow(Random.Range(0, 3)));

        }else if(Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ScanUpDown(2));
            StartCoroutine(ScanLeftToRight(1));
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(ScanUpDown(2));
        }
    }

    IEnumerator ShrinkCircleAt() {

        while (true)
        {
            int randomRow = Random.Range(0, 3);
            int randomColumn = Random.Range(0, columnsCount - 1);
            Vector2 point = positions[columnsCount * randomRow + randomColumn];

            float startTime = Time.time;
            while (Time.time < startTime + pingDuration)
            {
                float progress = Time.time / (startTime + pingDuration);
                Laser.Instance.AddCircleData(
                    laserIdx: 1,
                    patternID: 2,
                    brightness: CONST.LASER_MAX_VALUE,
                    wobble: 0,
                    rotation_speed: Vector3.zero,
                    pointsMultiplier: 1,
                    center: point,
                    radius: gridCellSize/2f * progress
                    );
                yield return null;
            }

            Laser.Instance.AddCircleData(
              laserIdx: 1,
              patternID: 2,
              brightness: 0,
              wobble: 0,
              rotation_speed: Vector3.zero,
              pointsMultiplier: 1,
              center: point,
              radius: 0
              );
        }

    }


    IEnumerator PingDot(int id, int column, int row) {

        int randomRow = row;// Random.Range(0, 3);
                                // StartCoroutine(GlitchRow(randomRow));
            int randomColumn = column; //Random.Range(0, columns - 1);
            Vector2 point = positions[columnsCount * randomRow + randomColumn];

            float startTime = Time.time;
            while (Time.time < startTime + pingDuration)
            {
                float progress = (Time.time - startTime) / pingDuration;
                //print(progress);
                Laser.Instance.AddCircleData(
                    laserIdx: 1,
                    patternID: id,
                    brightness: CONST.LASER_MAX_VALUE,
                    wobble: .0002f,
                    rotation_speed: Vector3.zero,
                    pointsMultiplier: 1f * (1f - progress),
                    center: point,
                    radius: pingSize * progress
                    );
                yield return null;
            }
            //HOLD
        Laser.Instance.AddCircleData(
                laserIdx: 1,
                patternID: id,
                brightness: CONST.LASER_MAX_VALUE,
                wobble: .0002f,
                rotation_speed: Vector3.zero,
                pointsMultiplier: holdPointsMultiplier,
                center: point,
                radius: pingSize
            );

         yield return new WaitForSeconds(pingHOLD);

        Laser.Instance.AddCircleData(
                laserIdx: 1,
                patternID: id,
                brightness: 0,
                wobble: .0005f,
                rotation_speed: Vector3.zero,
                pointsMultiplier: 1,
                center: point,
                radius: pingSize
            );      
    }

    IEnumerator ScanLeftToRight(int id) {
        float startPoint_x = positions[0].x - gridCellSize / 2f;
        float endPoint_x = positions[positions.Count - 1].x + gridCellSize / 2f;

        float start_y = positions[0].y - gridCellSize / 2f;
        float end_y = positions[positions.Count - 1].y + gridCellSize;

        //SAME X, DIFF Y
        Vector2 startPoint = new Vector2(startPoint_x, start_y);
        Vector2 endPoint = new Vector2(startPoint_x, end_y);




        float startTime = Time.time;
        while (Time.time < startTime + pingDuration)
        {
            float progress = (Time.time - startTime) / pingDuration;
            float new_x = Mathf.Lerp(startPoint_x, endPoint_x, progress);

            startPoint.x = new_x;
            endPoint.x = new_x;

            Laser.Instance.AddLineData(
                laserIdx: 1,
                patternID: id,
                startPoint: startPoint,
                endPoint: endPoint,
                brightness: CONST.LASER_MAX_VALUE,
                wobble: 0
                );


            yield return null;
        }

        Laser.Instance.AddLineData(
              laserIdx: 1,
              patternID: id,
              startPoint: startPoint,
              endPoint: endPoint,
              brightness: 0,
              wobble: 0
             );
    }

    IEnumerator ScanUpDown(int id)
    {
        float startPoint_x = positions[0].x - gridCellSize / 2;
        float endPoint_x = positions[positions.Count - 1].x + gridCellSize;

        float start_y = positions[0].y - gridCellSize;
        float end_y = positions[positions.Count - 1].y + gridCellSize;

        Vector2 startPoint = new Vector2(startPoint_x, start_y);
        Vector2 endPoint = new Vector2(endPoint_x, start_y);




        float startTime = Time.time;
        while (Time.time < startTime + pingDuration)
        {
            float progress = (Time.time - startTime) / pingDuration;
            float new_y = Mathf.Lerp(start_y, end_y, progress);
            startPoint.y = new_y;
            endPoint.y = new_y;

            Laser.Instance.AddLineData(
                laserIdx: 1,
                patternID: id,
                startPoint: startPoint,
                endPoint: endPoint,
                brightness: CONST.LASER_MAX_VALUE,
                wobble: 0
                );


            yield return null;
        }

        Laser.Instance.AddLineData(
              laserIdx: 1,
              patternID: id,
              startPoint: startPoint,
              endPoint: endPoint,
              brightness: 0,
              wobble: 0
             );
    }

    void OnValidate()
    {

        if (!sendData) return;

        Laser.Instance.ClearSquares();

        positions.Clear();


        for (int row = 0; row < rowsCount; row++)
        {
            for (int column = 0; column < columnsCount; column++)
            {
                Vector2 cellPosition = new Vector2(column * gridCellSize, row * gridCellSize);
                positions.Add(cellPosition);
            }
        }

        for (int i = 0; i < positions.Count; i++) {
            Laser.Instance.AddSquareData(
                laserIdx: 1,
                patternID: i,
                rotation_speed: Vector3.zero,
                center: positions[i],
                sideLength: gridCellSize/2f,
                pointsMultiplier: 1,
                brightness: (ushort)(CONST.LASER_MAX_VALUE * gridBrightNess),
                dashLength: 0,
                wobble: 0
                );
            //int(positions[row]);
        }
    }
}
