using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Physics2D�̊g���N���X
/// </summary>
public static class Physics2DExtentsion
{

    //Ray�̕\������
    private const float RAY_DISPLAY_TIME = 3;

    /// <summary>
    /// Ray���΂��Ɠ����ɉ�ʂɐ���`�悷��
    /// </summary>
    public static RaycastHit2D RaycastAndDraw(Vector2 origin, Vector2 direction, float maxDistance, int layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

        //�Փˎ���Ray����ʂɕ\��
        if (hit.collider)
        {
            Debug.DrawRay(origin, hit.point - origin, Color.blue, RAY_DISPLAY_TIME, false);
        }
        //��Փˎ���Ray����ʂɕ\��
        else
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.green, RAY_DISPLAY_TIME, false);
        }

        return hit;
    }

}