// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "PooledObject.h"
#include "Bullet.generated.h"

class UProjectileMovementComponent;

UCLASS()
class REUSABLESYSTEMS_API ABullet : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ABullet();

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Pooling", meta = (AllowPrivateAccess = "true"))
	UPooledObject* PoolComp;

protected:
	
	virtual void BeginPlay() override;

public:
	// Called every frame

	UFUNCTION(BlueprintCallable, Category = "Bullet")
	void ActivateBullet();

	UFUNCTION(BlueprintCallable, Category = "Bullet")
	void DeactivateBullet();

};
