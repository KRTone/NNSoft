#pragma once
#include <iostream>
#include <exception>
using namespace std;

class NullPointerException : public exception
{
public:
    NullPointerException(const char* msg) : std::exception(msg)
    {

    }
};