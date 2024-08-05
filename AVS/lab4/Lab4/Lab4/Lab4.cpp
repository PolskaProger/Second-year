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

    __asm {
        ; sub B from A(convert from 8 to 16 bit to avaid overflow)
        lea eax, A
        lea ebx, B

        movsd xmm0, [eax]
        movsd xmm1, [ebx]

        xorpd xmm2, xmm2
        xorpd xmm3, xmm3

        pcmpgtb xmm2, xmm0
        pcmpgtb xmm3, xmm1

        punpcklbw xmm0, xmm2
        punpcklbw xmm1, xmm3

        psubw xmm0, xmm1
        ; result is in xmm0

        ; adding C to prev result
        lea eax, C
        movsd xmm1, [eax]
        xorpd xmm2, xmm2

        pcmpgtb xmm2, xmm1
        punpcklbw xmm1, xmm2

        paddw xmm0, xmm1
        ; result is in xmm0

        ; sub D from prev result(convert from 16 to 32 bit to avaid overflow)
        movupd xmm1, xmm0
        xorpd xmm2, xmm2

        pcmpgtw xmm2, xmm0

        punpcklwd xmm0, xmm2
        punpckhwd xmm1, xmm2

        lea eax, D
        movupd xmm2, [eax]
        movupd xmm3, xmm2
        xorpd xmm4, xmm4

        pcmpgtw xmm4, xmm2

        punpcklwd xmm2, xmm4
        punpckhwd xmm3, xmm4

        psubd xmm0, xmm2
        psubd xmm1, xmm3
        ; result is in xmm0 - xmm1

        lea eax, F
        movupd[eax], xmm0
        movupd[eax + 16], xmm1
    }

    std::cout << "Final array F (type of data: int32):\n";
    for (int i = 0; i < 8; i++) {
        std::cout << F[i] << " ";
    }
    return 0;
}
