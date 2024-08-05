#include <iostream>

int main()
{
    __int8 A[8], B[8], C[8];
    __int16 D[8];
    __int32 F[8];

    std::cout << "F[i] = A[i] - B[i] + C[i] - D[i] , i=1...8\n";

    std::cout << "Enter array A (type of data: int8)\n";
    for (int i = 0; i < 8; i++) {
        int temp;
        std::cin >> temp;
        A[i] = (__int8)temp;
    }

    std::cout << "Enter array B (type of data: int8)\n";
    for (int i = 0; i < 8; i++) {

        int temp;
        std::cin >> temp;
        B[i] = (__int8)temp;
    }

    std::cout << "Enter array C (type of data: int8)\n";
    for (int i = 0; i < 8; i++) {
        int temp;
        std::cin >> temp;
        C[i] = (__int8)temp;
    }

    std::cout << "Enter array D (type of data: int16)\n";
    for (int i = 0; i < 8; i++) {
        std::cin >> D[i];
    }

    __int64 buf[2];

    __asm {
        ; sub B from A(convert from 8 to 16 bit to avaid overflow)
        lea eax, A
        lea ebx, B

        movq mm0, [eax]
        movq mm1, [ebx]
    
        movq mm2, mm0
        movq mm3, mm1
        pxor mm4, mm4
        pxor mm5, mm5

        pcmpgtb mm4, mm2
        pcmpgtb mm5, mm3

        punpcklbw mm0, mm4
        punpcklbw mm1, mm5
        punpckhbw mm2, mm4
        punpckhbw mm3, mm5

        psubw mm0, mm1
        psubw mm2, mm3
        movq mm1, mm2

        lea eax, C
        movq mm2, [eax]
        movq mm3, mm2
        pxor mm4, mm4

        pcmpgtb mm4, mm2

        punpcklbw mm2, mm4
        punpckhbw mm3, mm4
        paddw mm0, mm2
        paddw mm1, mm3

        pxor mm2, mm2
        pxor mm3, mm3
        pcmpgtw mm2, mm0
        pcmpgtw mm3, mm1

        movq mm4, mm0
        movq mm5, mm0
        movq mm6, mm1
        movq mm7, mm1

        punpcklwd mm4, mm2
        punpckhwd mm5, mm2
        punpcklwd mm6, mm3
        punpckhwd mm7, mm3

        lea eax, D
        movq mm0, [eax]
        movq mm1, [eax + 8]
        pxor mm2, mm2
        pxor mm3, mm3

        pcmpgtw mm2, mm0
        pcmpgtw mm3, mm1

        lea ebx, buf
        movq[ebx], mm2
        movq[ebx + 8], mm3
        movq mm2, mm1
        movq mm1, mm0
        movq mm3, mm2

        punpcklwd mm0, [ebx]
        punpckhwd mm1, [ebx]
        punpcklwd mm2, [ebx + 8]
        punpckhwd mm3, [ebx + 8]

        psubd mm4, mm0
        psubd mm5, mm1
        psubd mm6, mm2
        psubd mm7, mm3

        lea eax, F
        movq[eax], mm4
        movq[eax + 8], mm5
        movq[eax + 16], mm6
        movq[eax + 24], mm7
    }

    std::cout << "Final array F (type of data: int32):\n";
    for (int i = 0; i < 8; i++) {
        std::cout << F[i] << " ";
    }
    return 0;
}
