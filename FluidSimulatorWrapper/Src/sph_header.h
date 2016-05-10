
#pragma once

#if UNITY_EDITOR || UNITY_STANDALONE
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
