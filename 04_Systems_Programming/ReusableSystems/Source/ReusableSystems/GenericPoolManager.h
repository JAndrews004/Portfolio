// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "PooledObject.h"
#include "GenericPoolManager.generated.h"


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class REUSABLESYSTEMS_API UGenericPoolManager : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UGenericPoolManager();

	UFUNCTION(BlueprintCallable, Category = "Object Pool")
	UPooledObject* SpawnPooledObject();

	UPROPERTY(EditAnywhere, Category = "Object Pool")
	TSubclassOf<class AActor> PooledObjectSubclass;

	UPROPERTY(EditAnywhere, Category = "Object Pool")
	int PoolSize = 20;

	UPROPERTY(EditAnywhere, Category = "Object Pool")
	float PooledObjectLifeSpan = 0.0f;

	UFUNCTION()
	void OnPooledObjectDespawn(UPooledObject* PoolActor);


protected:
	// Called when the game starts
	virtual void BeginPlay() override;
	TArray<UPooledObject*> ObjectPool;
	TArray<int> SpawnedPoolIndexes;
};
