#!/bin/bash

BASE_URL="http://localhost:5208"

echo ""
echo "=========================================="
echo "  REDIS CACHE DEMO - TEST SCRIPT"
echo "=========================================="
echo ""

echo "TEST 1: Debug - Checking Cache Type"
echo "-----------------------------------"
curl -s $BASE_URL/api/products/debug
echo ""
echo ""

echo "TEST 2: Get Product 1 - FIRST REQUEST (expect CACHE MISS)"
echo "----------------------------------------------------------"
curl -s $BASE_URL/api/products/1
echo ""
echo ""
sleep 1

echo "TEST 3: Get Product 1 - SECOND REQUEST (expect CACHE HIT)"
echo "----------------------------------------------------------"
curl -s $BASE_URL/api/products/1
echo ""
echo ""
sleep 1

echo "TEST 4: Get All Products"
echo "------------------------"
curl -s $BASE_URL/api/products
echo ""
echo ""
sleep 1

echo "TEST 5: Invalidate Cache for Product 1"
echo "---------------------------------------"
curl -s -X DELETE $BASE_URL/api/products/cache/1
echo ""
echo ""
sleep 1

echo "TEST 6: Get Product 1 AFTER Invalidation (expect CACHE MISS again)"
echo "-------------------------------------------------------------------"
curl -s $BASE_URL/api/products/1
echo ""
echo ""

echo "=========================================="
echo "  TESTS COMPLETE - Check app logs above!"
echo "=========================================="
