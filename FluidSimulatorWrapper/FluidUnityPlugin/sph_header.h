<<<<<<< HEAD
#pragma once

#if UNITY_EDITOR || UNITY_STANDALONE
=======
#if UNITY_METRO
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
#define EXPORT_API __declspec(dllexport) __stdcall
#elif UNITY_WIN
#define EXPORT_API __declspec(dllexport)
#else
#define EXPORT_API
#endif

#ifndef __SPHHEADER_H__
#define __SPHHEADER_H__

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>
#include <sstream>

#define PI 3.141592f
#define INF 1E-12f
#define BOUNDARY 0.0001f


#endif
